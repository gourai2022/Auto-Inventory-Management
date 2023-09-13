using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyProject.Models;
using System.Collections.Generic;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection.Metadata.Ecma335;

namespace MyProject.Controllers;

public class HomeController : Controller
{
    public readonly WmiService _wmiService = new WmiService();
    
    //private readonly List<honda_wmi> wmiData;
    public List<honda_wmi> wmiData = new List<honda_wmi>();
    public string _jsonFilePath = "honda_wmi.json";
    public string? SearchQuery { get; set; }
    //List<string?> distinctCountries = new List<string?>();
    private readonly ILogger<HomeController> _logger;
    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
        //wmiData = new List<honda_wmi>();
    }
    public IActionResult Index()
    {
        wmiData = new List<honda_wmi>();
        wmiData = _wmiService.LoadWmiDataFromJsonFile();
        IEnumerable<string?> distinctCountries = wmiData.Select(item => item.Country).Distinct().ToList();
        // Pass the distinct values to the view
        ViewBag.Countries = distinctCountries;
        wmiData = _wmiService.LoadWmiDataFromJsonFile();
        //wmiData = wmiData.OrderByDescending(item => item.CreatedOn).ThenBy(item => item.WMI).ToList();
            
        return View(wmiData);
    }
   
    [HttpPost]
    public IActionResult Index(string searchQuery, string groupByCountry)
    {
        //wmiData = LoadWmiDataFromJsonFile();
        ///SearchQuery = search.ToString();
        //wmiData = FilterData(SearchQuery);
        wmiData = _wmiService.LoadWmiDataFromJsonFile();
        // Set ViewBag values for the view
        ViewBag.SearchQuery = searchQuery;
        ViewBag.Countries = wmiData.Select(item => item.Country).Distinct().ToList();
        wmiData = filterBySearchCountry(searchQuery, groupByCountry);
        return View(wmiData);
    }
    public IActionResult SortTable()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    /*private List<honda_wmi> LoadWmiDataFromJsonFile()
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
    }*/
    private List<honda_wmi> FilterData(String SearchQuery)
    {
        if (string.IsNullOrWhiteSpace(SearchQuery))
        {
            // If the search query is empty or null, return the original data.
            return new List<honda_wmi>();
        }
        
        // Convert the search query to lowercase for case-insensitive search
        //SearchQuery = SearchQuery.ToLowerInvariant();
        Console.WriteLine($"SearchQuery: {SearchQuery}");

        if (wmiData != null &&  wmiData.Count > 0)    
        {
            Console.WriteLine($"wmiData.Count: {wmiData.Count}");
         
            List<honda_wmi> filtered_WMI_list = wmiData
                .Where(item =>
                    (item.CreatedOn != null && item.CreatedOn.Contains(SearchQuery)) ||
                    (item.DateAvailableToPublic != null && item.DateAvailableToPublic.Contains(SearchQuery)) ||
                    (item.Name != null && item.Name.Contains(SearchQuery)) ||
                    (item.UpdatedOn != null && item.UpdatedOn.Contains(SearchQuery)) ||
                    (item.VehicleType != null && item.VehicleType.Contains(SearchQuery)) ||
                    (item.WMI != null && item.WMI.Contains(SearchQuery))).ToList();

            
            Console.WriteLine($"filtered_WMI_list.Count: {filtered_WMI_list.Count}");
            return filtered_WMI_list;
        }
        else
        {
            // Handle the case where deserialization returns null, possibly due to invalid JSON.
            Console.WriteLine("wmiData is null.");
        }
        return new List<honda_wmi>();
    }

    public List<honda_wmi> filterBySearchCountry(string searchQuery, string groupByCountry)
    {
        wmiData = _wmiService.LoadWmiDataFromJsonFile();
        
        // Set ViewBag values for the view
        ViewBag.SearchQuery = searchQuery;
        ViewBag.Countries = wmiData.Select(item => item.Country).Distinct().ToList();

        // Filter by search query
        if (!string.IsNullOrWhiteSpace(searchQuery))
        {
            wmiData = wmiData
                .Where(item =>
                    (item.CreatedOn != null && item.CreatedOn.Contains(searchQuery)) ||
                    (item.DateAvailableToPublic != null && item.DateAvailableToPublic.Contains(searchQuery)) ||
                    (item.Name != null && item.Name.Contains(searchQuery)) ||
                    (item.UpdatedOn != null && item.UpdatedOn.Contains(searchQuery)) ||
                    (item.VehicleType != null && item.VehicleType.Contains(searchQuery)) ||
                    (item.WMI != null && item.WMI.Contains(searchQuery)))
                .ToList();
        }

        // Filter by selected country
        if (!string.IsNullOrEmpty(groupByCountry))
        {
            wmiData = wmiData.Where(item => item.Country == groupByCountry).ToList();
        }

        // Rest of your sorting and data processing logic

        return wmiData;
    }
}
