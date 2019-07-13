using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Rain4.Models;

namespace Rain4.Controllers
{
    public class IzpitiController : Controller
    {
        private static List<Izpit> seznamIzpitov = new List<Izpit>();
        public IActionResult Index()
        {
            ViewBag.podatki = "";
            return View();
        }

        public string listToString(List<Izpit> list)
        {
            string listed = "";
            Izpit it;
            for(int i=0; i<list.Count; i++)
            {
                it = list[i];
                listed += it.ToString() + "\n";
            }
            return listed;
        }

        public IActionResult Dodaj(string ime, string priimek, string izpit)
        {
            Izpit novIzpit = new Izpit();

            novIzpit.ime = ime;
            novIzpit.priimek = priimek;
            novIzpit.imeIzpita = izpit;
            novIzpit.datum = DateTime.Now;

            seznamIzpitov.Add(novIzpit);

            ViewBag.podatki = listToString(seznamIzpitov);

            return View("Index");
        }

        public IActionResult Prenesi(string izvozi)
        {

            if (izvozi == "XML")
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Izpit>));
                using (var writer = new StreamWriter("prijaveStudentov_" + DateTime.Now.ToString("mm.ss") + "_" + DateTime.Now.ToString("dd.mm.yyyy") + ".xml"))
                {
                    serializer.Serialize(writer, seznamIzpitov);
                    writer.Close();
                }
                /*
                Repeater1.DataSource = seznam;
                Repeater1.DataBind();
                */
            }
            else if (izvozi == "JSON")
            {

                JsonSerializer serializer = new JsonSerializer();
                string output = JsonConvert.SerializeObject(seznamIzpitov);
                serializer.NullValueHandling = NullValueHandling.Ignore;
                string path = "prijaveStudentov_" + DateTime.Now.ToString("HH.mm") + "_" + DateTime.Now.ToString("dd.mm.yyyy") + ".json";
                using (StreamWriter sw = new StreamWriter(path))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, seznamIzpitov);
                    // {"ExpiryDate":new Date(1230375600000),"Price":0}
                }
            }
            return View("Index");
        }

        public IActionResult Nalozi(IFormFile file)
        {
            var result = string.Empty;

            string filename = file.FileName;
            int filenameSize = filename.Length;
            if (filename.Substring(filenameSize - 4) == ".xml") //pogledaš če je pred pred predzadnji element . potem je xml drugače je .json
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Izpit>));
                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    seznamIzpitov = (List<Izpit>)serializer.Deserialize(reader);
                }
            }
            else if (filename.Substring(filenameSize - 5) == ".json")
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Izpit>));
                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    result = reader.ReadToEnd();
                }
                seznamIzpitov = JsonConvert.DeserializeObject<List<Izpit>>(result);
            }

            return View("Index");
        }
    }
}