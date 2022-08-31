namespace HazGo.Application.ValidationMessage
{
    public static class ValidationMessage
    {
        public const string UniqueEmail = "Email must be unique";
        public const string NameExist = "Name already exist";
        public const string UniqueMobile = "Mobile must be unique";
        public const string MustbeRequiredContactType = "Applicant and Finance contact Type must be Required";
        public const string RegionRequired = "Region is Required";
        public const string DocumentRequired = "Document is Required";
        public const string UniqueRegionCode = "Region code must be unique";
        public const string UniqueRegionId = "Region Id must be unique";
        public const string AddressMustBeFromMasterLookup = "AddressType Must Be From Master";
        public const string ContactMustBeFromMasterLookup = "ContactType Must Be From Master";
        public const string DocumentMustBeFromMasterLookup = "DocumentType Must Be From Master";

        public const int MaxAllowedAddressCount = 5;
        public const int MaxAllowedRegionCount = 10;
        public const int MaxAllowedContactCount = 5;
        public const int MaxAllowedReferenceCount = 5;
        public const int MaxAllowedDocumentCount = 5;
    }
}
