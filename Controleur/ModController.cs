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

        public void initLocalRepositoryFromExistingFolder()
        {
            if (Directory.Exists(m.path))
            {
                try
                {
                    repository = new Repository(m.path);
                    //Commands.Fetch(repository,"origin",,new FetchOptions { TagFetchMode= TagFetchMode.All});
                    //Commands.Checkout(repository, LastTag.Target.Sha);
                    m.Tags = repository.Tags;


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

            Console.WriteLine(m.path);

            if (Directory.Exists(m.path))
            {
                
                try
                {
                    m.status = "repertoire trouvé...";
                    Console.WriteLine(m.path);
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
                //creation du depot 
            }

        }
        public void setstatus(string s)
        {
            m.status = s;
        }
        public async Task<string> getDataFromApi()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(m.apiUrl);
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.UserAgent.TryParseAdd("request2");


                HttpResponseMessage response = await client.GetAsync(m.apiUrl).ConfigureAwait(false);

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

        public async void getLastTagNameReleaseFromRepo()
        {
            try
            {
                string data = await getDataFromApi().ConfigureAwait(false);
                if (data == null )
                {
                    m.versionDisponible = "Impossible de recupéré la derniére version dispo";
                }
                else
                {
                    JsonApiGitReturn returnApi = JsonConvert.DeserializeObject<JsonApiGitReturn>(data);
                    m.versionDisponible = returnApi.name;
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
