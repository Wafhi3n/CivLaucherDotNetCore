using CivLaucherDotNetCore.Model;
using CivLauncher;
using LibGit2Sharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
namespace CivLaucherDotNetCore.Controleur
{
    public class ModController
    {

        Repository repository;
        public Mod m;
        public Tag tagSelect { get; set; }

        public void initLocalRepositoryFromExistingFolder()
        {

            if (Directory.Exists(m.path))
            {
                try
                {
                    repository = new Repository(m.path);
                    Commands.Fetch(repository, "origin", new[] { "+refs/heads/*:refs/remotes/origin/*" },new FetchOptions { TagFetchMode= TagFetchMode.All},"fetchtag");;
                    //Commands.Checkout(repository, LastTag.Target.Sha);
                    //m.Tags = repository.Tags.ToList<Tag>();


                    foreach (Tag tag in repository.Tags)
                    {
                        if (tag.Reference.TargetIdentifier == repository.Head.Tip.Id.Sha)
                        {
                            m.version = tag.FriendlyName;
                                

                        }

                    }


                }catch(Exception ex)
                {
                    m.status = "erreur repo";
                 }
            }

                

            //getLastTagNameFromRepo();

            //repo.Tags.First();
            //Console.WriteLine("Tag ? :"+repo.Head.Tip.Id);




            //verification de la version du mod 
        }

        internal void updateOrInstallToLastTag(Object selectedItem)
        {
            Tag selectedTag = (Tag)selectedItem;
            //Console.WriteLine(selectedTag);
            if (Directory.Exists(m.path))
            {
                if (selectedItem != null)
                {
                   
                    Commands.Checkout(repository, selectedTag.Target.Sha);
                    m.version = selectedTag.FriendlyName;
                }
                else
                {
                    Tag LastTag = repository.Tags.Last();
                    Commands.Checkout(repository, LastTag.Target.Sha);
                    m.version = LastTag.FriendlyName;
                }

            }
            else
            {
                cloneMod();
            }
        }

        internal void updateBranchToTagClick()
        {
            updateBranchToTag(tagSelect);
        }

        internal void updateBranchToTag(Tag t)
        {
            
            if (Directory.Exists(m.path))
            {
                if ( t != null)
                {

                    Commands.Checkout(repository, t.Target.Sha);
                    m.version = t.FriendlyName;
                }
                else
                {
                    Tag LastTag = m.tags.Last();
                    Commands.Checkout(repository, LastTag.Target.Sha);
                    m.version = LastTag.FriendlyName;
                }

            }
            else
            {
                cloneMod();
            }
        }

        internal void cloneMod()
        {
            Directory.CreateDirectory(m.path);
            Repository.Clone( m.repoUrl+"/"+m.repositoriInfo.owner+"/"+m.repositoriInfo.depot, m.path);
            repository = new Repository(m.path);
            //Commands.Fetch()
            Tag LastTag = repository.Tags.Last();
            Commands.Checkout(repository, LastTag.Target.Sha);
            m.version = LastTag.FriendlyName;
            m.status = "OK";
            //m.Tags = m.Tags;
        }

            public ModController(CivLauncher.Mod m)
        {
            this.m = m;
            if (Directory.Exists(m.path))
            {
                repository = new Repository(m.path);
                getReleaseTagsFromApi("/releases");
                try
                {
                    m.status = "repertoire trouvé...";
                }
                catch (Exception)
                {
                    Console.WriteLine("Error reading app settings");
                }
           
                Console.WriteLine("dossier: " + m.path + " trouvé");
            }
            else
            {
                m.status = "KO";

                Console.WriteLine("dossier: " + m.path + " non trouvé");
            }

        }
        public async Task<string> getDataFromApi(string call)
        {
            using (var client = new HttpClient())
            {
                Console.WriteLine(m.apiUrl+call);
                client.BaseAddress = new Uri(m.apiUrl + call);
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.UserAgent.TryParseAdd("request2");


                HttpResponseMessage response = await client.GetAsync(m.apiUrl + call).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {

                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    return null;
                }
            }

        }
        public async void getReleaseTagsFromApi(string call)
        {
            try
            {
               string data = await getDataFromApi("/releases").ConfigureAwait(false);
                if (data == null)
                {
                    Console.WriteLine(data);

                }
                else
                {
                   var returnApis = JsonConvert.DeserializeObject<List<JsonApiGitReturnLastRelease>>(data);
                    Lookup<string,Tag> tg = (Lookup<string, Tag>)repository.Tags.ToLookup(t => t.FriendlyName);
                    Lookup<string, JsonApiGitReturnLastRelease> tg2 = (Lookup<string, JsonApiGitReturnLastRelease>)returnApis.ToLookup(t=>t.tag_name);
                    foreach (IGrouping<string, JsonApiGitReturnLastRelease> tag2 in tg2)
                    {
                        foreach (IGrouping<string, Tag> tag in tg)
                        {
                            if(tag2.Key == tag.Key)
                            {
                                m.tags.Add(tag.First());
                                continue;
                            }
                        }
                    }
                }
            }
            catch (TaskCanceledException ex)
            {
                throw ex;
            }
        }
        public async void getLastTagNameReleaseFromRepo()
        {
            try
            {
                string data = await getDataFromApi("/releases/latest").ConfigureAwait(false);
                if (data == null )
                {
                }
                else
                {
                    JsonApiGitReturnLastRelease returnApi = JsonConvert.DeserializeObject<JsonApiGitReturnLastRelease>(data);
                    Console.WriteLine(m.version);
                }
            }
            catch (TaskCanceledException ex)
            {
                throw ex;
            }
            

        }

    }
}
