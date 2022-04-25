using CivLaucherDotNetCore.Model;
using CivLaucherDotNetCore.Vue.Model;
using LibGit2Sharp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
namespace CivLaucherDotNetCore.Controleur
{
    public class ModController
    {

        public ObservableCollection<Tag> tags;

        internal ModView View { get; set; }

        public Tag LastTag { get; set; }

        public string InstalledVersion()
        {
            if (repository != null) return repository.Head.Tip.Sha;
            else return "";

        }
        public string derniereVersionDisponible
        {

            get
            {
                if (this.tags !=null && this.tags.Count > 0 && this.tags[0] != null)
                {
                    return this.tags.First().FriendlyName;
                }
                else { return ""; }
            }

            set
            {

            }

        }
        public Tag TagActuel()
        {
            foreach (Tag tag in repository.Tags)
            {
                if (tag.Reference.TargetIdentifier == repository.Head.Tip.Id.Sha)
                {
                    this.View.tagSelect = tag;
                    return tag;

                }

            }
            return null;
        }
        public int IndexVersionActuel
        {
            get
            {
                foreach (var obj in repository.Tags.Select((value, index) => new { index, value }))
                {
                   
                    if (obj.value.Reference.TargetIdentifier == repository.Head.Tip.Id.Sha)
                    {

                        return  obj.index;
                    }
                }
                return 0;
            }
        }

        private Repository repository { get; set; }


        private Mod m { get; set; }
        public Tag tagSelect { get; set; }
        public ModController(Mod m)
        {
            this.m = m;
            this.tags = new ObservableCollection<Tag>();
            if (isInstalled())
            {
                initLocalRepositoryFromExistingFolder();
                getReleaseTagsFromApi();
            }
            else
            {
            }

        }

        public Boolean isInstalled()
        {
            if (m.IsInstalled())
            {
                try
                {
                    Repository rp = new Repository(m.path);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return false;
        }
        public void initLocalRepositoryFromExistingFolder()
        {
            try
            {
                repository = new Repository(m.path);
                Commands.Fetch(repository, "origin", new[] { "+refs/heads/*:refs/remotes/origin/*" }, new FetchOptions { TagFetchMode = TagFetchMode.All }, "fetchtag"); ;
            }
            catch (Exception ex)
            {
                m.status = "erreur repo";
            }
        }
        internal async Task updateBranchToTagAsync(Tag t)
        {

            if (isInstalled())
            {
                if (t != null)
                {

                    Commands.Checkout(repository, t.Target.Sha);
                    //m.version = t.FriendlyName;
                }
                else
                {
                    Tag LastTag = tags.Last();
                    Commands.Checkout(repository, LastTag.Target.Sha);
                    //m.version = LastTag.FriendlyName;
                }

            }
            else
            {
                await cloneMod();
            }
        }

        internal async Task cloneMod()
        {
            Directory.CreateDirectory(m.path);
            Repository.Clone(m.repoUrl + "/" + m.repoOwner + "/" + m.repoName, m.path);
            repository = new Repository(m.path);
            await getReleaseTagsFromApi();
            //LastTag = tags.First();
            Commands.Checkout(repository, LastTag.Target.Sha);

         }


        public async Task<string> getDataFromApi(string call)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(m.apiUrl + call);
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.UserAgent.TryParseAdd("request");
                HttpResponseMessage response = await client.GetAsync(m.apiUrl + call).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    string retour = await response.Content.ReadAsStringAsync();
                    storeDataJson(call, retour);
                    return retour;
                }
                else
                {
                    return null;
                }
            }

        }

        public void storeDataJson(string call, string data)
        {
            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/ModLoader/cacheJson/"))
            {
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/ModLoader/cacheJson/");
            }
            string retourJson = JsonConvert.SerializeObject(data);
            File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/ModLoader/cacheJson/" + callJsonName(call), retourJson);
        }
        public string callJsonName(string call)
        {
            return call + m.repoOwner + m.repoName + ".json";
        }

        public async Task<string> loadStoredDataJsonAsync(string call)
        {
            string retour = "";
            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/ModLoader/cacheJson/"))
            {
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/ModLoader/cacheJson/");

            }
            else
            {
                string uriFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/ModLoader/cacheJson/" + callJsonName(call);
                if (File.Exists(uriFile))
                {
                    DateTime modification = File.GetLastWriteTime(uriFile).AddMinutes(30);
                    if (DateTime.Compare(modification, DateTime.Now) > 0)
                    {
                        string fichier = File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/ModLoader/cacheJson/" + callJsonName(call));
                        retour = JsonConvert.DeserializeObject<string>(fichier);
                    }
                    else
                    {
                        retour = await getDataFromApi(call).ConfigureAwait(false);
                    }
                }
            }
            return retour;
        }
        public async Task getReleaseTagsFromApi()
        {//            LastTag = tags.First();


            if (this.isInstalled())
            {
                try
                {
                    string data;
                    string storedData = await loadStoredDataJsonAsync("/releases");

                    if (storedData != null && storedData != "")
                    {
                        data = storedData;
                    }
                    else
                    {
                        data = await getDataFromApi("/releases").ConfigureAwait(false);

                    }

                    var returnApis = JsonConvert.DeserializeObject<List<JsonApiGitReturnLastRelease>>(data);
                    Lookup<string, Tag> tg = (Lookup<string, Tag>)repository.Tags.ToLookup(t => t.FriendlyName);
                    Lookup<string, JsonApiGitReturnLastRelease> tg2 = (Lookup<string, JsonApiGitReturnLastRelease>)returnApis.ToLookup(t => t.tag_name);
                    foreach (IGrouping<string, JsonApiGitReturnLastRelease> tag2 in tg2)
                    {
                        foreach (IGrouping<string, Tag> tag in tg)
                        {
                            if (tag2.Key == tag.Key)
                            {
                                App.Current.Dispatcher.Invoke((Action)delegate //<--- HERE
                                {
                                    tags.Add(tag.First());
                                });

                                continue;
                            }
                        }
                    }
                    LastTag = tags.First();

                }
                catch (TaskCanceledException ex)
                {
                    throw ex;
                }
            }
        }
        public async void getLastTagNameReleaseFromRepo()
        {
            try
            {
                string data = await getDataFromApi("/releases/latest").ConfigureAwait(false);
                if (data == null)
                {
                }
                else
                {
                    JsonApiGitReturnLastRelease returnApi = JsonConvert.DeserializeObject<JsonApiGitReturnLastRelease>(data);
                }
            }
            catch (TaskCanceledException ex)
            {
                throw ex;
            }


        }

    }
}
