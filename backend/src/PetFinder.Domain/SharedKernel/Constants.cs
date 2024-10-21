// ReSharper disable InconsistentNaming

namespace PetFinder.Domain.SharedKernel;

public static class Constants
{
    
    
    public static class PetPhoto
    {
        public const int MaxPathLength = 256;
        public const int MaxNameLength = 128;
        public const string TableName = "pet_photos";
        public const string BucketName = "petfinder.photos";
    }

    public static class Volunteer
    {
        public const int MaxFirstNameLength = 32;
        public const int MaxMiddleNameLength = 32;
        public const int MaxLastNameLength = 32;
        public const int MaxDescriptionLength = 256;
        public const int MinExperienceYears = 0;
        public const string TableName = "volunteers";
    }

    public static class PhoneNumber
    {
        public const int MaxLength = 16;
    }

    public static class Email
    {
        public const int MaxLength = 256;
    }

    public static class Pet
    {
        public const int MaxNameLength = 128;
        public const int MaxGeneralDescriptionLength = 256;
        public const int MaxBreedLength = 32;
        public const int MaxColorLength = 32;
        public const int MaxHealthInformationLength = 256;
        public const int MaxOwnerPhoneNumberLength = 16;
        public const string TableName = "pets";
        public const double MinWeightValue = 0.1d;
        public const double MinHeightValue = 0.01d;
        public const int MinOrderNumber = 1;
    }

    public static class Address
    {
        public const int MaxCountryLength = 64;
        public const int MaxCityLength = 64;
        public const int MaxStreetLength = 64;
        public const int MaxHouseLength = 16;
        public const int MaxDescriptionLength = 64;
    }

    public static class SocialNetwork
    {
        public const int MaxUrlLength = 256;
        public const int MaxTitleLength = 32;
    }

    public static class AssistanceDetail
    {
        public const int MaxTitleLength = 64;
        public const int MaxDescriptionLength = 128;
    }

    public static class PersonName
    {
        public const int MaxFirstNameLength = 64;
        public const int MaxMiddleNameLength = 64;
        public const int MaxLastNameLength = 64;
    }

    public static class Species
    {
        public const int MaxTitleLength = 64;
        public const string TableName = "species";
    }

    public static class Breed
    {
        public const int MaxTitleLength = 64;
        public const int MaxDescriptionLength = 128;
        public const string TableName = "breed";
    }

    public static class FileExtension
    {
        public static class Images
        {
            public const string Jpg = ".jpg";
            public const string Jpeg = ".jpeg";
            public const string Png = ".png";

            public static bool Contains(string ext)
            {
                ext = ext.ToLower();
                
                return ext == Jpg
                       || ext == Jpeg 
                       || ext == Png;
            }
        }

        public const int MaxPathLength = 1024;
        public const int MaxNameLength = 128;
    }
}