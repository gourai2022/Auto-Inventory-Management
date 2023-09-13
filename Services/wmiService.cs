using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyProject.Models;
using System.Collections.Generic;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection.Metadata.Ecma335;

public class WmiService
{   
    public string _jsonFilePath = "honda_wmi.json";
    public List<honda_wmi> LoadWmiDataFromJsonFile()
    {
        try
        {
            if (_jsonFilePath != null)
            {
                //string json = System.IO.File.ReadAllText(_jsonFilePath);
                //var hondaWmiList = JsonSerializer.Deserialize<List<honda_wmi>>(json);
                string json = System.IO.File.ReadAllText(_jsonFilePath);
                var hondaWmiList = JsonConvert.DeserializeObject<List<honda_wmi>>(json);
                if (hondaWmiList != null)
                {
                    return hondaWmiList;
                }
                else
                {
                    // Handle the case where deserialization returns null, possibly due to invalid JSON.
                    Console.WriteLine("Deserialization returned null.");
                }
            }
            else
            {
                // Handle the case where the file does not exist.
                Console.WriteLine("JSON file does not exist.");
            }
        }
        catch (Exception ex)
        {
            // Log the error and consider rethrowing the exception if needed.
            Console.WriteLine("Error reading JSON file: " + ex.Message);
            // Consider rethrowing the exception to handle it at a higher level.
            // throw;
        }
        // Return an empty list or handle the error case as needed.
        return new List<honda_wmi>();
    }
}