LibTXReSciCollCSharp is a .NET library that assists with creation of item-level metadata files for the USGS ReSciColl scientific sample collection database.

It can be installed as a NuGet package or by cloning this repository and referencing the project in your code.  Once you have it included in your project, it can be used like this:

```
using LibTXReSciCollCSharp;

//Create an item collection object
RSCItems rSCItems = new RSCItems();

//Create a sample, which comes from an oil and gas well.  This type has the API number, (well) operator, lease name, etc. attributes.
RSCRockCoreFromOGWell sample = new RSCRockCoreFromOGWell();

//Set the properties of the item.
sample.LocalID = "000001"; //Per the USGS docs, this is the primary key from your database, or some other unique identifier.
sample.Title = "Sample with ID 000001"; //This should be 
sample.CoordinatesLatitude = 28.45554997499842;
sample.CoordinatesLongitude = -99.75494921582204;
sample.ApiNumber = "4212700001";
sample.LeaseName = "Bob's Oilfield Services";
sample.WellNumber = "#1";
sample.CountyName = "Dimmit";
sample.StateName = "Texas";
sample.CountryName = "USA";
sample.SampleType = "Whole Core";
sample.VerticalExtentTop = 5000;
sample.VerticalExtentBottom = 10000;
sample.VerticalExtentUnit = "ft";
sample.OnlineResource = "http://yourwebsite.com/samples/" + "000001";
sample.Igsn = "BEG00000001";

//Add the item to our collection
rSCItems.AddItem(sample);

//Write the item-level metadata file to disk.
rSCItems.ToRSCCsvFile(".\\sampleoutput.csv");

```

A functioning copy of this example is included in the LibTXReSciCollCSharpUsageExample.  Also, a more comprehensive example, which is the actual code used by the Bureau of Economic Geology to export its own item-level metadata.