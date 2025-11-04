using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using DesignAutomationFramework;
using System;
using System.IO;

namespace RevitPlugin
{
	[Transaction(TransactionMode.Manual)]
	[Regeneration(RegenerationOption.Manual)]
	public class Commands : IExternalDBApplication
	{
		//Path of the output file
		string OUTPUT_FILE = "geodata.json";

		public ExternalDBApplicationResult OnStartup(ControlledApplication application)
		{
			DesignAutomationBridge.DesignAutomationReadyEvent += HandleDesignAutomationReadyEvent;
			return ExternalDBApplicationResult.Succeeded;
		}

		private void HandleDesignAutomationReadyEvent(object? sender, DesignAutomationReadyEventArgs e)
		{
			LogTrace("Design Automation Ready event triggered...");
			e.Succeeded = true;
			ExportGeo(e.DesignAutomationData.RevitDoc);
		}

        private void ExportGeo(Document doc)
        {
            //get Site Location
            SiteLocation siteLocation = doc.ActiveProjectLocation.GetSiteLocation();
            //Create a JSON object with the site location and project location
            var jsonObject = new {
                siteLocation = new {
                    latitude = siteLocation.Latitude,
                    longitude = siteLocation.Longitude,
                    placeName = siteLocation.PlaceName,
                    elevation = siteLocation.Elevation,
                    geoCoordinateSystemDefinition = siteLocation.GeoCoordinateSystemDefinition
                }
            };
            //Save the JSON object to the output file
            File.WriteAllText(OUTPUT_FILE, jsonObject.ToString());
        }

		/// <summary>
		/// This will appear on the Design Automation output
		/// </summary>
		private static void LogTrace(string format, params object[] args) { System.Console.WriteLine(format, args); }

        public ExternalDBApplicationResult OnShutdown(ControlledApplication application)
        {
            return ExternalDBApplicationResult.Succeeded;
        }
    }
}