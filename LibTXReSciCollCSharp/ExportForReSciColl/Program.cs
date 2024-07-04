//This is the actual program the Bureau of Economic Geology uses for exporting items from its internal collections database, called Continuum.
//This program is not provided with the intention that it be used as-is, but rather to provide a more comprehensive example of how to use LibTXReSciCollCSharp in a real-world scenario.

using LibTXReSciCollCSharp;
using LibContinuumBackend;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Data.SqlClient;
using static System.Formats.Asn1.AsnWriter;
using LibContinuumBackend.DBTypes;

void setItemWellProperties(LibTXReSciCollCSharp.RSCSampleFromOGWell item, LibContinuumBackend.DBTypes.Well Well)
{
    item.ApiNumber = Well.ApiNumber;
    item.LeaseName = Well.LeaseName;
    item.WellNumber = Well.WellNumber;
    item.CountyName = Well.CountyName;
    item.StateName = Well.StateName;
    item.CountryName = Well.CountryName;
    item.CoordinatesLongitude = Well.SurfaceLongitude;
    item.CoordinatesLatitude = Well.SurfaceLatitude;

    if (Well.FieldName != null && Well.FieldName != string.Empty) {
        item.FieldName = Well.FieldName;
    }

    if (Well.OperatorName != null)
    {
        item.OperatorName = Well.OperatorName;
    }

    if (Well.CoordsUncertain)
    {
        item.CoordsUncertain = true;
        item.CoordsUncertainDescription = "County centroid";
    }
}

//Create an EFCore context to connect to our source database
LibContinuumBackend.DBTypes.Continuum2Context ctx = new LibContinuumBackend.DBTypes.Continuum2Context();

//Each of these values can be set to true to in order to enable exporting of that data type.
bool doCores = false;
bool doCuttings = false;
bool doThinSections = false;
bool doGeophysicalLogs = false;
bool doCorePhotos = false;
bool doCoreReports = false;
bool doCoreChips = false;
bool doHandSamples = false;
bool doGISDatasets = false;
bool doCriticalMineralsInventories = false;
bool doThinSectionScans = true;

//First, do the cores.
if (doCores)
{
    var cores = (from s in ctx.Samples
                 where s.SampleType != null && 
                        s.SampleType.Contains("core") &&
                        s.Well.SurfaceLatitude != null &&
                        s.Well.SurfaceLongitude != null &&
                        s.Deaccessioned == false &&
                        s.UseRestricted == false
                 select s).Include("Well");

    LibTXReSciCollCSharp.RSCItems coreItems = new LibTXReSciCollCSharp.RSCItems();

    int itemCount = 0;

    foreach (var core in cores)
    {
        LibTXReSciCollCSharp.RSCRockCoreFromOGWell rscCore = new RSCRockCoreFromOGWell();

        rscCore.LocalID = core.SampleId.ToString();

        setItemWellProperties(rscCore, core.Well);

        rscCore.VerticalExtentUnit = "ft";
        rscCore.VerticalExtentTop = core.TopDepth;
        rscCore.VerticalExtentBottom = core.BottomDepth;
        rscCore.SampleType = core.SampleType;

        if(core.Formation != null && core.Formation !=string.Empty)
        {
            rscCore.FormationName = core.Formation;
        }

        if(core.FormationAge != null && core.FormationAge != string.Empty)
        {
            rscCore.FormationAge = core.FormationAge;
        }
        
        if (core.Igsn != null && core.Igsn != string.Empty)
        {
            rscCore.Igsn = core.Igsn;
        }

        if (core.AccessionNumber != null && core.AccessionNumber != string.Empty)
        {
            rscCore.AlternateTitle = core.AccessionNumber;
        }

        if (core.CollectionDate != null)
        {
            rscCore.Date = core.CollectionDate;
        }

        rscCore.OnlineResource = "https://coastal.beg.utexas.edu/continuum/#!/sample/" + core.SampleId.ToString();

        rscCore.Title = "Sample ID " + core.SampleId;

        coreItems.AddItem(rscCore);

        itemCount++;
    }

    coreItems.ToRSCCsvFile("cores.csv");
}

if (doCuttings)
{

    //Next, do the cuttings

    var cuttings = (from s in ctx.Samples
                    where s.SampleType != null && 
                        s.SampleType.Contains("cuttings") && 
                        s.Well.SurfaceLatitude != null &&
                        s.Well.SurfaceLongitude != null &&
                        s.Deaccessioned == false &&
                        s.UseRestricted == false
                    select s).Include("Well");

    LibTXReSciCollCSharp.RSCItems cuttingsItems = new LibTXReSciCollCSharp.RSCItems();

    foreach (var core in cuttings)
    {
        LibTXReSciCollCSharp.RSCCuttingsFromOGWell rscCore = new RSCCuttingsFromOGWell();

        rscCore.LocalID = core.SampleId.ToString();

        setItemWellProperties(rscCore, core.Well);

        rscCore.VerticalExtentUnit = "ft";
        rscCore.VerticalExtentTop = core.TopDepth;
        rscCore.VerticalExtentBottom = core.BottomDepth;
        rscCore.SampleType = core.SampleType;

        if (core.Formation != null && core.Formation != string.Empty)
        {
            rscCore.FormationName = core.Formation;
        }

        if (core.FormationAge != null && core.FormationAge != string.Empty)
        {
            rscCore.FormationAge = core.FormationAge;
        }

        if (core.Igsn != null && core.Igsn != string.Empty)
        {
            rscCore.Igsn = core.Igsn;
        }

        if (core.AccessionNumber != null && core.AccessionNumber != string.Empty)
        {
            rscCore.AlternateTitle = core.AccessionNumber;
        }

        if (core.CollectionDate != null)
        {
            rscCore.Date = core.CollectionDate;
        }

        rscCore.OnlineResource = "https://coastal.beg.utexas.edu/continuum/#!/sample/" + core.SampleId.ToString();

        rscCore.Title = "Sample ID " + core.SampleId;

        cuttingsItems.AddItem(rscCore);
    }

    cuttingsItems.ToRSCCsvFile("cuttings.csv");
}

if (doThinSections)
{
    //Next, do the thin sections
    var thinSections = (from s in ctx.Samples
                    where 
                        s.SampleType != null &&
                        s.SampleType.Contains("thin") && 
                        s.SampleType.Contains("section") && 
                        s.Well.SurfaceLatitude != null && 
                        s.Well.SurfaceLongitude != null &&
                        s.Deaccessioned == false && 
                        s.UseRestricted == false

                    select s).Include("Well");

    LibTXReSciCollCSharp.RSCItems thinSectionItems = new LibTXReSciCollCSharp.RSCItems();

    foreach (var core in thinSections)
    {
        LibTXReSciCollCSharp.RSCThinSectionFromOGWell rscThinSection = new RSCThinSectionFromOGWell();

        rscThinSection.LocalID = core.SampleId.ToString();

        setItemWellProperties(rscThinSection, core.Well);

        rscThinSection.VerticalExtentUnit = "ft";
        rscThinSection.VerticalExtentTop = core.TopDepth;
        rscThinSection.VerticalExtentBottom = core.BottomDepth;
        rscThinSection.SampleType = core.SampleType;

        if (core.Formation != null && core.Formation != string.Empty)
        {
            rscThinSection.FormationName = core.Formation;
        }

        if (core.FormationAge != null && core.FormationAge != string.Empty)
        {
            rscThinSection.FormationAge = core.FormationAge;
        }

        if (core.Igsn != null && core.Igsn != string.Empty)
        {
            rscThinSection.Igsn = core.Igsn;
        }

        if (core.AccessionNumber != null && core.AccessionNumber != string.Empty)
        {
            rscThinSection.AlternateTitle = core.AccessionNumber;
        }

        if (core.CollectionDate != null)
        {
            rscThinSection.Date = core.CollectionDate;
        }

        rscThinSection.OnlineResource = "https://coastal.beg.utexas.edu/continuum/#!/sample/" + core.SampleId.ToString();

        rscThinSection.Title = "Sample ID " + core.SampleId;

        thinSectionItems.AddItem(rscThinSection);
    }

    thinSectionItems.ToRSCCsvFile("thin_sections.csv");
}

if (doHandSamples)
{
    var handSamples = (from c in ctx.Samples
                     where c.SampleType != null && 
                     c.SampleType.ToLower().Contains("hand") &&
                     c.Well.SurfaceLatitude != null &&
                     c.Well.SurfaceLongitude != null &&
                     c.Deaccessioned == false &&
                     c.UseRestricted == false
                     select c
                     ).Include("Well");

    LibTXReSciCollCSharp.RSCItems iHandSamples = new LibTXReSciCollCSharp.RSCItems();

    foreach (var handSampleRec in handSamples)
    {
        LibTXReSciCollCSharp.RSCHandSpecimen rscHandSpecimen = new LibTXReSciCollCSharp.RSCHandSpecimen();

        rscHandSpecimen.LocalID = handSampleRec.SampleId.ToString();

        setItemWellProperties(rscHandSpecimen, handSampleRec.Well);

        rscHandSpecimen.VerticalExtentUnit = "ft";
        rscHandSpecimen.VerticalExtentTop = handSampleRec.TopDepth;
        rscHandSpecimen.VerticalExtentBottom = handSampleRec.BottomDepth;

        if (handSampleRec.Formation != null && handSampleRec.Formation != string.Empty)
        {
            rscHandSpecimen.FormationName = handSampleRec.Formation;
        }

        if (handSampleRec.FormationAge != null && handSampleRec.FormationAge != string.Empty)
        {
            rscHandSpecimen.FormationAge = handSampleRec.FormationAge;
        }

        if (handSampleRec.Igsn != null && handSampleRec.Igsn != string.Empty)
        {
            rscHandSpecimen.Igsn = handSampleRec.Igsn;
        }

        if (handSampleRec.AccessionNumber != null && handSampleRec.AccessionNumber != string.Empty)
        {
            rscHandSpecimen.AlternateTitle = handSampleRec.AccessionNumber;
        }

        if (handSampleRec.CollectionDate != null)
        {
            rscHandSpecimen.Date = handSampleRec.CollectionDate;
        }

        rscHandSpecimen.OnlineResource = "https://coastal.beg.utexas.edu/continuum/#!/sample/" + handSampleRec.SampleId.ToString();

        rscHandSpecimen.Title = "Sample ID " + handSampleRec.SampleId;

        iHandSamples.AddItem(rscHandSpecimen);
    }

    iHandSamples.ToRSCCsvFile("handsamples.csv");
}

if(doCoreChips)
{
    var coreChips = (from c in ctx.Samples
                     where c.SampleType != null && 
                     c.SampleType.ToLower().Contains("chips") &&
                     c.SampleType.ToLower().Contains("cuttings") == false &&
                     c.Well.SurfaceLatitude != null &&
                     c.Well.SurfaceLongitude != null &&
                     c.Deaccessioned == false &&
                     c.UseRestricted == false
                     select c
                     ).Include("Well");

    LibTXReSciCollCSharp.RSCItems iCoreChips = new LibTXReSciCollCSharp.RSCItems();

    foreach(var coreChipRec in  coreChips)
    {
        LibTXReSciCollCSharp.RSCCoreChips rscCoreChips = new LibTXReSciCollCSharp.RSCCoreChips();

        rscCoreChips.LocalID = coreChipRec.SampleId.ToString();

        setItemWellProperties(rscCoreChips, coreChipRec.Well);

        rscCoreChips.VerticalExtentUnit = "ft";
        rscCoreChips.VerticalExtentTop = coreChipRec.TopDepth;
        rscCoreChips.VerticalExtentBottom = coreChipRec.BottomDepth;

        if (coreChipRec.Formation != null && coreChipRec.Formation != string.Empty)
        {
            rscCoreChips.FormationName = coreChipRec.Formation;
        }

        if (coreChipRec.FormationAge != null && coreChipRec.FormationAge != string.Empty)
        {
            rscCoreChips.FormationAge = coreChipRec.FormationAge;
        }

        if (coreChipRec.Igsn != null && coreChipRec.Igsn != string.Empty)
        {
            rscCoreChips.Igsn = coreChipRec.Igsn;
        }

        if (coreChipRec.AccessionNumber != null && coreChipRec.AccessionNumber != string.Empty)
        {
            rscCoreChips.AlternateTitle = coreChipRec.AccessionNumber;
        }

        if (coreChipRec.CollectionDate != null)
        {
            rscCoreChips.Date = coreChipRec.CollectionDate;
        }

        rscCoreChips.OnlineResource = "https://coastal.beg.utexas.edu/continuum/#!/sample/" + coreChipRec.SampleId.ToString();

        rscCoreChips.Title = "Sample ID " + coreChipRec.SampleId;

        iCoreChips.AddItem(rscCoreChips);
    }

    iCoreChips.ToRSCCsvFile("corechips.csv");
}

if(doCorePhotos)
{
    var corePhotos = (from l in ctx.Logs
                where l.MaterialType == "Core Photos" &&
                    l.Well.SurfaceLatitude != null &&
                    l.Well.SurfaceLongitude != null &&
                    l.NotForDistribution == false
                select l).Include("Well").Include("LogScans");

    LibTXReSciCollCSharp.RSCItems iCorePhotos = new LibTXReSciCollCSharp.RSCItems();

    foreach (var log in corePhotos)
    {
        LibTXReSciCollCSharp.RSCCorePhoto rscLog = new RSCCorePhoto();

        rscLog.LocalID = log.LogId.ToString();

        setItemWellProperties(rscLog, log.Well);

        rscLog.VerticalExtentUnit = "ft";
        rscLog.VerticalExtentTop = log.TopDepth;
        rscLog.VerticalExtentBottom = log.BottomDepth;

        if (log.AccessionNumber != null && log.AccessionNumber != string.Empty)
        {
            rscLog.AlternateTitle = log.AccessionNumber.Trim();
        }

        if (log.LogDate != null)
        {
            rscLog.Date = log.LogDate;
        }

        int freeScansCount = 0;
        int paidScansCount = 0;

        var freescans = from s in log.LogScans
                        where s.AllowFreeDownload = true
                        select s;

        freeScansCount = freescans.Count();

        var paidscans = from s in log.LogScans
                        where s.AllowFreeDownload == false
                        select s;

        paidScansCount = paidscans.Count();

        rscLog.PaidScanCount = paidScansCount;
        rscLog.FreeScanCount = freeScansCount;

        rscLog.OnlineResource = "https://coastal.beg.utexas.edu/continuum/#!/log/" + log.LogId.ToString();

        if (log.Well.LeaseName != null && log.Well.WellNumber != null)
        {
            string title = "Core photos from " + log.Well.LeaseName + " " + log.Well.WellNumber + " - ID " + log.LogId.ToString();
            rscLog.Title = title;
        }
        else if (log.Well.ApiNumber != null)
        {
            string title = "Core photos from API#: " + log.Well.ApiNumber + " - ID " + log.LogId.ToString();
            rscLog.Title = title;
        }
        else 
        {
            rscLog.Title = "Core Photos " + log.LogId;
        }

        iCorePhotos.AddItem(rscLog);
    }

    iCorePhotos.ToRSCCsvFile("corephotos.csv");
}

if(doCoreReports)
{
    var coreReports = (from l in ctx.Logs
                      where l.MaterialType == "Core Report" &&
                          l.Well.SurfaceLatitude != null &&
                          l.Well.SurfaceLongitude != null &&
                          l.NotForDistribution == false
                      select l).Include("Well").Include("LogScans");

    LibTXReSciCollCSharp.RSCItems iCoreReports = new LibTXReSciCollCSharp.RSCItems();

    foreach (var log in coreReports)
    {
        LibTXReSciCollCSharp.RSCCorePhoto rscLog = new RSCCorePhoto();

        rscLog.LocalID = log.LogId.ToString();

        setItemWellProperties(rscLog, log.Well);

        rscLog.VerticalExtentUnit = "ft";
        rscLog.VerticalExtentTop = log.TopDepth;
        rscLog.VerticalExtentBottom = log.BottomDepth;


        if (log.AccessionNumber != null && log.AccessionNumber != string.Empty)
        {
            rscLog.AlternateTitle = log.AccessionNumber.Trim();
        }

        if (log.LogDate != null)
        {
            rscLog.Date = log.LogDate;
        }

        int freeScansCount = 0;
        int paidScansCount = 0;

        var freescans = from s in log.LogScans
                        where s.AllowFreeDownload = true
                        select s;

        freeScansCount = freescans.Count();

        var paidscans = from s in log.LogScans
                        where s.AllowFreeDownload == false
                        select s;

        paidScansCount = paidscans.Count();

        rscLog.PaidScanCount = paidScansCount;
        rscLog.FreeScanCount = freeScansCount;

        rscLog.OnlineResource = "https://coastal.beg.utexas.edu/continuum/#!/log/" + log.LogId.ToString();

        rscLog.Title = "Core Report " + log.LogId;

        iCoreReports.AddItem(rscLog);
    }

    iCoreReports.ToRSCCsvFile("corereports.csv");
}

if (doGeophysicalLogs)
{
    //Next, do the cuttings

    var logs = (from l in ctx.Logs
                where l.MaterialType == "Log" &&
                    l.Well.SurfaceLatitude != null &&
                    l.Well.SurfaceLongitude != null &&
                    l.NotForDistribution == false
                select l).Include("Well").Include("LogScans");

    LibTXReSciCollCSharp.RSCItems logsItems = new LibTXReSciCollCSharp.RSCItems();

    foreach (var log in logs)
    {
        LibTXReSciCollCSharp.RSCGeophysicalLogFromOGWell rscLog = new RSCGeophysicalLogFromOGWell();

        rscLog.LocalID = log.LogId.ToString();

        setItemWellProperties(rscLog, log.Well);

        rscLog.VerticalExtentUnit = "ft";
        rscLog.VerticalExtentTop = log.TopDepth;
        rscLog.VerticalExtentBottom = log.BottomDepth;

        /*
        if (log.Igsn != null && log.Igsn != string.Empty)
        {
            rscLog.Igsn = log.Igsn;
        }*/

        if (log.AccessionNumber != null && log.AccessionNumber != string.Empty)
        {
            rscLog.AlternateTitle = log.AccessionNumber.Trim();
        }

        if (log.LogDate != null)
        {
            rscLog.Date = log.LogDate;
        }

        if (log.LogTypes != null)
        {
            rscLog.LogTypes = log.LogTypes;
        }

        if (log.LogScales != null)
        {
            rscLog.LogScales = log.LogScales;
        }

        if (log.LogNumber != null)
        {
            rscLog.LogNumber = log.LogNumber;
        }

        if (log.LogSource != null)
        {
            rscLog.LogSource = log.LogSource;
        }

        if (log.MaxRecordedTemperature != null)
        {
            rscLog.MaxRecordedTemperature = log.MaxRecordedTemperature;
        }

        if (log.MaxRecordedTemperature2 != null)
        {
            rscLog.MaxRecordedTemperature2 = log.MaxRecordedTemperature2;
        }

        if (log.MaxRecordedTemperature3 != null)
        {
            rscLog.MaxRecordedTemperature3 = log.MaxRecordedTemperature3;
        }

        if (log.CirculationTimeText != null)
        {
            rscLog.CirculationTime = log.CirculationTimeText;
        }

        int freeScansCount = 0;
        int paidScansCount = 0;

        var freescans = from s in log.LogScans
                        where s.AllowFreeDownload = true
                        select s;

        freeScansCount = freescans.Count();

        var paidscans = from s in log.LogScans
                        where s.AllowFreeDownload == false
                        select s;

        paidScansCount = paidscans.Count();

        rscLog.PaidScanCount = paidScansCount;
        rscLog.FreeScanCount = freeScansCount;

        rscLog.OnlineResource = "https://coastal.beg.utexas.edu/continuum/#!/log/" + log.LogId.ToString();

        rscLog.Title = "Geophysical Log ID " + log.LogId;

        logsItems.AddItem(rscLog);
    }

    logsItems.ToRSCCsvFile("logs.csv");
}

if(doGISDatasets)
{
    var logRecs = (from l in ctx.Logs
                where l.MaterialType == "Geographic Map (GIS) Datasets" &&
                    l.Well.SurfaceLatitude != null &&
                    l.Well.SurfaceLongitude != null &&
                    l.NotForDistribution == false
                select l).Include("Well").Include("LogScans");

    LibTXReSciCollCSharp.RSCItems logsItems = new LibTXReSciCollCSharp.RSCItems();

    foreach(var log in logRecs)
    {
        LibTXReSciCollCSharp.RSCGISDataset item = new RSCGISDataset();

        item.LocalID = log.LogId.ToString();

        item.CoordinatesLatitude = log.Well.SurfaceLatitude;
        item.CoordinatesLongitude = log.Well.SurfaceLongitude;

        item.CoordsUncertain = log.Well.CoordsUncertain;
        item.CoordsUncertainDescription = "Extent centroid";

        item.Description = log.Description;

        item.PublicationDate = log.BatchDate;
        item.Date = log.LogDate;
        item.OnlineResource = "https://coastal.beg.utexas.edu/continuum/#!/log/" + log.LogId.ToString();
        item.Title = "GIS Dataset " + log.LogId;

        logsItems.AddItem(item);
    }

    logsItems.ToRSCCsvFile("GisDatasets.csv");
}

if(doCriticalMineralsInventories)
{
    var logRecs = (from l in ctx.Logs
                   where l.MaterialType == "Critical Minerals Inventory" &&
                       l.Well.SurfaceLatitude != null &&
                       l.Well.SurfaceLongitude != null &&
                       l.NotForDistribution == false
                   select l).Include("Well").Include("LogScans");

    LibTXReSciCollCSharp.RSCItems logsItems = new LibTXReSciCollCSharp.RSCItems();

    foreach (var log in logRecs)
    {
        LibTXReSciCollCSharp.RSCCriticalMineralsInventory item = new RSCCriticalMineralsInventory();

        item.LocalID = log.LogId.ToString();

        item.CoordinatesLatitude = log.Well.SurfaceLatitude;
        item.CoordinatesLongitude = log.Well.SurfaceLongitude;

        item.CoordsUncertain = log.Well.CoordsUncertain;
        item.CoordsUncertainDescription = "Extent centroid";

        item.Description = log.Description;

        item.PublicationDate = log.BatchDate;
        item.Date = log.LogDate;
        item.OnlineResource = "https://coastal.beg.utexas.edu/continuum/#!/log/" + log.LogId.ToString();
        item.Title = "Critical Minerals Inventory " + log.LogId;

        logsItems.AddItem(item);
    }

    logsItems.ToRSCCsvFile("CriticalMineralsInventories.csv");
}

if(doThinSectionScans)
{
    var coreReports = (from l in ctx.Logs
                       where l.MaterialType == "Thin Section Scans" &&
                           l.Well.SurfaceLatitude != null &&
                           l.Well.SurfaceLongitude != null &&
                           l.NotForDistribution == false
                       select l).Include("Well").Include("LogScans");

    LibTXReSciCollCSharp.RSCItems iCoreReports = new LibTXReSciCollCSharp.RSCItems();

    foreach (var log in coreReports)
    {
        LibTXReSciCollCSharp.RSCCorePhoto rscLog = new RSCCorePhoto();

        rscLog.DataType = "Thin section scans";

        rscLog.LocalID = log.LogId.ToString();

        setItemWellProperties(rscLog, log.Well);

        rscLog.VerticalExtentUnit = "ft";
        rscLog.VerticalExtentTop = log.TopDepth;
        rscLog.VerticalExtentBottom = log.BottomDepth;


        if (log.AccessionNumber != null && log.AccessionNumber != string.Empty)
        {
            rscLog.AlternateTitle = log.AccessionNumber.Trim();
        }

        if (log.LogDate != null)
        {
            rscLog.Date = log.LogDate;
        }

        int freeScansCount = 0;
        int paidScansCount = 0;

        var freescans = from s in log.LogScans
                        where s.AllowFreeDownload = true
                        select s;

        freeScansCount = freescans.Count();

        var paidscans = from s in log.LogScans
                        where s.AllowFreeDownload == false
                        select s;

        paidScansCount = paidscans.Count();

        rscLog.PaidScanCount = paidScansCount;
        rscLog.FreeScanCount = freeScansCount;

        rscLog.OnlineResource = "https://coastal.beg.utexas.edu/continuum/#!/log/" + log.LogId.ToString();
        
        if (log.Well.LeaseName != null && log.Well.WellNumber != null)
        {
            string title = "Thin section scans from " + log.Well.LeaseName + " " + log.Well.WellNumber + " - ID " + log.LogId.ToString();
            rscLog.Title = title;
        }
        else if (log.Well.ApiNumber != null)
        {
            string title = "Thin section scans from API#: " + log.Well.ApiNumber + " - ID " + log.LogId.ToString();
            rscLog.Title = title;
        }
        else
        {
            rscLog.Title = "Core Photos " + log.LogId;
        }

        iCoreReports.AddItem(rscLog);
    }

    iCoreReports.ToRSCCsvFile("thinsectionscans.csv");
}