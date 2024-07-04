using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using CsvHelper;
using CsvHelper.Configuration.Attributes;
using static System.Collections.Specialized.BitVector32;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LibTXReSciCollCSharp {

    /// <summary>
    /// Base item class for LibTXReSciCollCSharp - While it is possible to create valid metadata using just this class, and it is not declared as an abstract, it is intended to be extended.  Several such implementations are provided for common sample types.
    /// </summary>
    public class RSCItem
    {
        // Private fields
        private string _localID;
        private double? _coordinatesLatitude;
        private double? _coordinatesLongitude;
        private string? _title;
        private string? _alternateTitle;
        private string? _abstract;
        private string? _dataType;
        private DateTime? _publicationDate = DateTime.Now;
        private string _collectionID;
        private string? _alternateGeometry;
        private string? _onlineResource;
        private string? _browseGraphic;
        private DateTime? _date;
        private string? _verticalExtent;
        private double? _verticalExtentTop;
        private double? _verticalExtentBottom;
        private string? _verticalExtentUnit;
        private string? _igsn;
        private string? _parentIgsn;
        private string? _relIgsn;
        private string? _relationType;
        private string? _largerWorkCitation;
        private string _internalReferenceNumber1;
        private string _internalReferenceNumber2;
        private string _internalReferenceNumber1Description;
        private string _internalReferenceNumber2Description;
        private string _supplementalInformation;
        private string _repositoryPhoneNumber;
        private string _repositoryEmailAddress;
        private string _repositoryName;
        private string _repositoryPhysicalAddress;
        private string _repositoryMailingAddress;
        private string _supplementalInformationAdditional;
        private string _abstractMore;

        private bool _coordsUncertain;
        private string? _coordsUncertainDescription;

        private string? _attachmentFileName;

        // Public properties

        /// <summary>
        /// Identifier provided by the collection owner for uniquely identifying this item, such as a well API, database primary key, et cetera.
        /// </summary>
        [Name("localID")]
        [Index(0)]
        public string LocalID
        {
            get { return _localID; }
            set { _localID = value; }
        }

        /// <summary>
        /// Latitude in WGS84, presumably of the site where the sample was collected.
        /// </summary>
        [Name("coordinateLat_WGS84")]
        [Index(6)]
        public double? CoordinatesLatitude
        {
            get { return _coordinatesLatitude; }
            set { _coordinatesLatitude = value; }
        }


        /// <summary>
        /// Longitude in WGS84, presumably of the site where the sample was collected.
        /// </summary>
        [Name("coordinateLon_WGS84")]
        [Index(5)]
        public double? CoordinatesLongitude
        {
            get { return _coordinatesLongitude; }
            set { _coordinatesLongitude = value; }
        }

        /// <summary>
        /// The primary title for this item.
        /// </summary>
        [Name("title")]
        [Index(1)]
        public string? Title
        {
            get {
                string? ret = null;
                
                if(_title != null)
                {
                    ret = _title.Trim();
                }

                return ret;
            }
            set { _title = value; }
        }

        /// <summary>
        /// Collection owners may elect to provide an alternate tite
        /// </summary>
        [Name("alternateTitle")]
        [Index(2)]
        public string? AlternateTitle
        {
            get {
                string? ret = null;

                if(_alternateTitle != null)
                {
                    ret = _alternateTitle.Trim();
                }

                return ret;
            }
            set { _alternateTitle = value; }
        }

        /// <summary>
        /// Main description of the item.  This should be human readable and can contain anything you want.
        /// </summary>
        [Name("abstract")]
        [Index(3)]
        public string Abstract
        {
            get
            {
                if (!string.IsNullOrEmpty(_abstract))
                    return _abstract.Trim();
                else
                    return ComposeAbstract();
            }
            set { _abstract = value; }
        }

        /// <summary>
        /// A controlled vocabulary of data types.  This should come from the controlled vocabulary listed here:
        /// https://www.sciencebase.gov/vocab/categories?parentId=4f4e475ee4b07f02db47df22
        /// </summary>
        [Name("dataType")]
        [Index(4)]
        public string? DataType
        {
            get {

                string? ret = null;

                if (_dataType != null)
                {
                    ret = _dataType.Trim();
                }

                return ret;
            }
            set { _dataType = value; }
        }

        /// <summary>
        /// String representation of the publication date of the metadata.  Defaults to current date (because we're generating our metadata... today.)
        /// </summary>
        [Name("publicationDate")]
        [Index(7)]
        public string sPublicationDate
        {
            get
            {
                string ret = string.Empty;

                if (this.PublicationDate.HasValue)
                {
                    ret = this.PublicationDate.Value.ToString("yyyyMMdd");
                }
                
                return ret;
            }
        }

        /// <summary>
        /// Publication date of the metadata.  This is where you should set this, if you're not going to just rely on the default, which is today.
        /// </summary>
        [Ignore]
        public DateTime? PublicationDate
        {
            get { return _publicationDate; }
            set { _publicationDate = value; }
        }

        /// <summary>
        /// Alternate geometry of some other type.
        /// </summary>
        [Name("alternateGeometry")]
        [Index(8)]
        public string? AlternateGeometry
        {
            get {
                string? ret = null;
                
                if(_alternateGeometry != null)
                {
                    _alternateGeometry.Trim();
                }
                return ret;
            }
            set { _alternateGeometry = value; }
        }

        /// <summary>
        /// URL to an online resource related to this sample.
        /// </summary>
        [Name("onlineResource")]
        [Index(9)]
        public string? OnlineResource
        {
            get {
                string? ret = null;

                if(_onlineResource != null)
                {
                    ret = _onlineResource.Trim();
                }

                return ret;
            }
            set { _onlineResource = value; }
        }

        /// <summary>
        /// URL for the browse graphic
        /// </summary>
        [Name("browseGraphic")]
        [Index(10)]
        public string? BrowseGraphic
        {
            get {
                string? ret = null;

                if(_browseGraphic != null)
                {
                    ret = _browseGraphic.Trim();
                }

                return ret;
            }

            set { _browseGraphic = value; }
        }

        /// <summary>
        /// String representation of the date for use in the CSV.  Read-only.
        /// </summary>
        [Name("date")]
        [Index(11)]
        public string sDate
        {
            get
            {
                string ret = string.Empty;

                if(this.Date.HasValue)
                {
                    ret = this.Date.Value.ToString("yyyyMMdd");
                }

                return ret;
            }
        }

        /// <summary>
        /// Meaningful date associated with this item, such as when it was collected.
        /// </summary>
        [Ignore]
        public DateTime? Date
        {
            get { return _date; }
            set { _date = value; }
        }

        /// <summary>
        /// String value of the vertical extent for use in the csv output.  Ex: "ft,200,400"
        /// </summary>
        [Name("verticalExtent")]
        [Index(12)]
        public string? VerticalExtent
        {
            get {

                string? ret = null;

                if(_verticalExtent != null)
                {
                    ret = _verticalExtent.Trim();
                }

                return ret;
            }
            set { _verticalExtent = value; }
        }

        /// <summary>
        /// The top of the vertical extent, in whatever units are specified.
        /// </summary>
        [Ignore]
        public double? VerticalExtentTop
        {
            get { return _verticalExtentTop; }
            set
            {
                if (value < 0)
                {
                    _verticalExtentTop = 0;
                }
                else
                {
                    _verticalExtentTop = value;
                }
                
                UpdateVerticalExtent();
            }
        }

        /// <summary>
        /// The bottom of the vertical extent, in whatever units are specified.
        /// Note that this can't be negative.
        /// </summary>
        [Ignore]
        public double? VerticalExtentBottom
        {
            get { return _verticalExtentBottom; }
            set
            {
                if (value < 0)
                {
                    _verticalExtentBottom = 0;
                }
                else
                {
                    _verticalExtentBottom = value;
                }
                UpdateVerticalExtent();
            }
        }

        /// <summary>
        /// Units used in the vertical extent.  Must be "ft" or "m"
        /// </summary>
        [Ignore]
        public string? VerticalExtentUnit
        {
            get {

                string? ret = null;

                if (_verticalExtentUnit != null)
                {
                    ret = _verticalExtentUnit.Trim();
                }

                return ret;
            }

            set
            {
                if (value != null && value != "m" && value != "ft")
                {
                    throw new ArgumentException("Invalid vertical extent unit.");
                }
                _verticalExtentUnit = value;
                UpdateVerticalExtent();
            }
        }

        /// <summary>
        /// IGSN of this sample
        /// </summary>
        [Name("IGSN")]
        [Index(13)]
        public string? Igsn
        {
            get {
                string? ret = null;

                if(_igsn != null)
                {
                    ret = (_igsn.Trim());
                }

                return ret;
            }

            set { _igsn = value; }
        }

        /// <summary>
        /// IGSN of the parent sample, if this one is derived from another.
        /// </summary>
        [Name("parentIGSN")]
        [Index(14)]
        public string? ParentIgsn
        {
            get {
                string? ret = null;

                if(_parentIgsn != null)
                {
                    ret = _parentIgsn.Trim();
                }

                return ret;
            }
            set { _parentIgsn = value; }
        }

        /// <summary>
        /// IGSN of a related sample or subsample.  Only the IGSN is required, not the full address.
        /// </summary>
        [Name("relIGSN")]
        [Index(15)]
        public string? RelIgsn
        {
            get { 

                string? ret = null;

                if(_relIgsn != null)
                {
                    ret = _relIgsn.Trim();
                }

                return ret;
            }

            set { _relIgsn = value; }
        }

        /// <summary>
        /// Relationship of the IGSN to the largerWorkCitation.  Use the DataCite type vocabulary here:
        /// https://www.sciencebase.gov/vocab/vocabulary/611d428bbfff3461918aba6e
        /// </summary>
        [Name("relationType")]
        [Index(16)]
        public string? RelationType
        {
            get {

                string? ret = null;

                if(_relationType != null)
                {
                    ret = _relationType.Trim();
                }

                return ret;
            }

            set { _relationType = value; }
        }

        /// <summary>
        /// Physical sample connection to publication (DOI). The collection may document a larger
        /// set of samples with only a section used in a journal article.Only the DOI itself is
        /// required, not the full web link.
        /// </summary>
        [Name("largerWorkCitation")]
        [Index(17)]
        public string? LargerWorkCitation
        {
            get {
                string? ret = null;

                if(_largerWorkCitation != null)
                {
                    ret = _largerWorkCitation.Trim();
                }
                return ret;
            }

            set { _largerWorkCitation = value; }
        }

        /// <summary>
        /// Full name of file that is to be linked to the item via the file attachment process. A full folder/directory path is not required, just the filename and extension.Must match the name used during file attachment ingest
        /// </summary>
        [Name("attachmentFileName")]
        [Index(18)]
        public string? AttachmentFileName
        {
            get {

                string? ret = null;
                if(_attachmentFileName != null)
                {
                    ret = _attachmentFileName.Trim();
                }

                return ret;
            }

            set { 
                _attachmentFileName = value; 
            }
        }

        /// <summary>
        /// This is a boolean field indicating whether the coordinates given represent the actual location the item came from, or some other location, such as a county centroid.
        /// </summary>
        [Ignore]
        public bool CoordsUncertain
        {
            get
            {
                return _coordsUncertain;
            }

            set { 
                _coordsUncertain = value;
            }
        }

        /// <summary>
        /// String describing in human-readable language what the coordinates given actually mean, when they do not represent the actual location the sample came from.  If CoordsUncertain is false, leave this blank.
        /// </summary>
        [Ignore]
        public string? CoordsUncertainDescription
        {
            get
            {
                string? ret = null;

                if(_coordsUncertainDescription != null)
                {
                    ret = _coordsUncertainDescription.Trim();
                }

                return ret;
            }

            set
            {
                _coordsUncertainDescription = value;
            }
        }

        /// <summary>
        /// Text placed at the beginning of the abstract
        /// </summary>
        [Ignore]
        public string? AbstractBeginText { get; set; }

        /// <summary>
        /// Text placed at the end of the abstract
        /// </summary>
        [Ignore]
        public string? AbstractEndText { get; set; }

        /// <summary>
        /// The name of the repository where this sample is stored.
        /// </summary>
        [Ignore]
        public string? RepositoryName
        {
            get
            {
                return _repositoryName;
            }
            set
            {
                _repositoryName = value;
            }
        }

        /// <summary>
        /// Contact email address for the repository storing this item
        /// </summary>
        [Ignore]
        public string? RepositoryEmail
        {
            get
            {
                return _repositoryEmailAddress;
            }

            set
            {
                _repositoryEmailAddress = value;
            }
        }

        /// <summary>
        /// Contact phone number for the repository storing this item.
        /// </summary>
        [Ignore]
        public string? RepositoryPhoneNumber
        {
            get
            {
                return _repositoryPhoneNumber;
            }

            set
            {
                _repositoryPhoneNumber = value;
            }
        }

        /// <summary>
        /// Physical address of the repository where this sample is stored.
        /// </summary>
        [Ignore]
        public string? RepositoryPhysicalAddress
        {
            get
            {
                return _repositoryPhysicalAddress;
            }

            set
            {
                _repositoryPhysicalAddress = value;
            }
        }

        /// <summary>
        /// Mailing address of the repository where this sample is stored.
        /// </summary>
        [Ignore]
        public string? RepositoryMailingAddress
        {
            get
            {
                return _repositoryMailingAddress;
            }

            set
            {
                _repositoryMailingAddress = value;
            }
        }

        /// <summary>
        /// Name of the person or entity who collected this sample.
        /// </summary>
        [Ignore]
        public string? CollectedBy
        {
            get;
            set;
        }

        // Constructor
        public RSCItem()
        {
            // Initialize fields here if needed
        }

        /// <summary>
        /// Method to add an element and label to the set of items to be included in the abstract text
        /// </summary>
        /// <param name="element">The value to be shown to the user.</param>
        /// <param name="label">The name of the element, to be shown as the label in the list.</param>
        /// <param name="list">The list of properties to add to</param>
        public virtual void AddAbstractElement(string element, string label, List<string> list)
        {
            if (!string.IsNullOrEmpty(element) && !string.IsNullOrEmpty(label))
            {
                list.Add(label + ": " + element);
            }
        }

        /// <summary>
        /// Composes the list of abstract elements.
        /// </summary>
        /// <returns>List of strings, each one meant to be a line in the "abstract" value.</returns>
        protected virtual List<string> ComposeAbstractElements()
        {
            var ret = new List<string>();

            if(CoordsUncertain == true && CoordsUncertainDescription != null)
            {
                ret.Add("Coordinates given are not absolute location.  Instead, these coordinates represent: " +  CoordsUncertainDescription);
            }

            ret.Add(ComposeSupplementalInformation());

            return ret;
        }

        /// <summary>
        /// Composes the value for the "abstract" field, which is basically a longtext that can contain whatever you want.
        /// </summary>
        /// <returns></returns>
        protected virtual string ComposeAbstract()
        {
            string ret = "";

            // Build the list 
            var elementList = ComposeAbstractElements();

            ret = string.Join("\r\n", elementList);

            ret = AbstractBeginText + "\r\n" + ret;

            ret += AbstractEndText;

            return ret.Trim();
        }

        // Method to validate for CSV export
        private void ValidateForCsv()
        {
            // Make sure we have a title
            if (string.IsNullOrEmpty(_title))
            {
                throw new ArgumentException("Title is required and cannot be empty.");
            }

            // Make sure we have an abstract value
            if (string.IsNullOrEmpty(_abstract) && ComposeAbstract() == string.Empty)
            {
                throw new ArgumentException("Abstract is required and cannot be empty.");
            }

            // Validate dataset reference date field
            if (_publicationDate == null)
            {
                throw new ArgumentException("Dataset reference date is required and cannot be empty.");
            }

            if(_coordsUncertain == true && (_coordsUncertainDescription == string.Empty || _coordsUncertainDescription == null))
            {
                throw new ArgumentException("Coordinates uncertain is set to true, but no description is given.");
            }
        }

        /// <summary>
        /// Method to update the vertical extent property
        /// </summary>
        private void UpdateVerticalExtent()
        {
            if (!string.IsNullOrEmpty(_verticalExtentUnit) && _verticalExtentTop != null && _verticalExtentBottom != null)
            {
                _verticalExtent = $"{_verticalExtentUnit},{_verticalExtentBottom},{_verticalExtentTop}";
            }
            else
            {
                _verticalExtent = null;
            }
        }

        private void AddSupplementalInformationElement(string element, string label, List<string> list)
        {
            if(element != null && label != null)
            {
                list.Add(label + ": " + element);
            }
        }

        private List<string> ComposeSupplementalInformationElements()
        {
            List<string> ret = new List<string>();

            AddSupplementalInformationElement(_repositoryName, "Repository Name", ret);
            AddSupplementalInformationElement(_repositoryEmailAddress, "Repository Email Address", ret);
            AddSupplementalInformationElement(_repositoryPhoneNumber, "Repository Phone Number", ret);
            AddSupplementalInformationElement(_repositoryMailingAddress, "Repository Mailing Address", ret);
            AddSupplementalInformationElement(_repositoryPhysicalAddress, "Repository Physical Address", ret);
            AddSupplementalInformationElement(_supplementalInformationAdditional, "Additional Information", ret);

            return ret;
        }

        private string ComposeSupplementalInformation()
        {
            List<string> elementList = ComposeSupplementalInformationElements();

            return string.Join("\r\n", elementList);
        }

        /// <summary>
        /// Method to convert the object to an array for CSV export
        /// </summary>
        /// <returns>Array of strings, each representing one field in the output CSV</returns>
        public string[] ToRSCCsvRowArray()
        {
            // Validate and make sure we have all the required fields
            ValidateForCsv();

            List<string> parts = new List<string>();

            string sTitle = string.Empty;
            string sAlternateTitle = string.Empty;
            string sAbstract = string.Empty;
            string sCoordinatesLongitude = string.Empty;
            string sCoordinatesLatitude = string.Empty;
            string sAlternateGeometry = string.Empty;
            string sOnlineResource = string.Empty;
            string sBrowseGraphic = string.Empty;
            string sVerticalExtent = string.Empty;
            string sIGSN = string.Empty;
            string sParentIGSN = string.Empty;
            string sRelIGSN = string.Empty;
            string sRelationType = string.Empty;
            string sLargerWorkCitation = string.Empty;

            if (_title != null)
            {
                sTitle = _title;
            }

            if(_alternateTitle != null)
            {
                sAlternateTitle = _alternateTitle;
            }

            if(_abstract != null)
            {
                sAbstract = _abstract;
            }

            if (_coordinatesLongitude != null)
            {
                sCoordinatesLongitude = _coordinatesLongitude.Value.ToString();
            }

            if (_coordinatesLatitude != null)
            {
                sCoordinatesLatitude = _coordinatesLatitude.Value.ToString();
            }

            if (_alternateGeometry != null)
            {
                sAlternateGeometry = _alternateGeometry.ToString();
            }

            if(_onlineResource != null)
            {
                sOnlineResource = _onlineResource.ToString();
            }

            if (_browseGraphic != null)
            {
                sBrowseGraphic = _browseGraphic.ToString();
            }

            if(_verticalExtent != null)
            {
                sVerticalExtent = _verticalExtent.ToString();
            }

            if(_igsn != null)
            {
                sIGSN = _igsn.ToString();
            }

            if(_parentIgsn != null)
            {
                sParentIGSN = _parentIgsn.ToString();
            }

            if(_relIgsn != null)
            {
                sRelIGSN = _relIgsn.ToString();
            }

            if(_relationType != null)
            {
                sRelationType = _relationType.ToString();
            }

            parts.Add(_localID);
            parts.Add(sTitle);
            parts.Add(sAlternateTitle);
            parts.Add(sAbstract);
            parts.Add(sCoordinatesLongitude);
            parts.Add(sCoordinatesLatitude);

            if (_publicationDate != null)
            {
                parts.Add(_publicationDate.Value.ToString("yyyyMMdd"));
            }
            else
            {
                parts.Add(string.Empty);
            }

            parts.Add(sAlternateGeometry);
            parts.Add(sOnlineResource);
            parts.Add(sBrowseGraphic);

            if (_date != null)
            {
                parts.Add(_date.Value.ToString("yyyyMMdd"));
            }
            else
            {
                parts.Add(null);
            }

            parts.Add(sVerticalExtent);
            parts.Add(sIGSN);
            parts.Add(sParentIGSN);
            parts.Add(sRelIGSN);
            parts.Add(sRelationType);
            parts.Add(sRelationType);

            return parts.ToArray();
        }
    }
}