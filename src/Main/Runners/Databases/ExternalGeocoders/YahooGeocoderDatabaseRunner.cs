using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
//using Reimers.Map.Geocoding;
using USC.GISResearchLab.Common.Addresses;
using USC.GISResearchLab.Common.Core.Databases;
using USC.GISResearchLab.Common.Core.Geocoders.GeocodingQueries.Options;
using USC.GISResearchLab.Geocoding.Core.ExternalGeocoders.Yahoo;
using USC.GISResearchLab.Geocoding.Core.OutputData;

namespace USC.GISResearchLab.Geocoding.Core.Runners.Databases
{
    public class YahooGeocoderDatabaseRunner : NonParsedGeocoderDatabaseRunner
    {

        public const string FIELD_NAME_ADDRESS = "ADDRESS";

        #region Geocoder Properties

        #endregion

        #region Constructors

        public YahooGeocoderDatabaseRunner()
            : base()
        {
            RunnerName = "Yahoo";
        }

        public YahooGeocoderDatabaseRunner(DataProviderType webStatusDataProviderStatus, DatabaseType webStatusDatabaseType, string webStatusConnectionString)
            : base(webStatusDataProviderStatus, webStatusDatabaseType, webStatusConnectionString)
        {
            RunnerName = "Yahoo";
        }

        public YahooGeocoderDatabaseRunner(TraceSource traceSource)
            : base(traceSource)
        {
            RunnerName = "Yahoo";
        }

        public YahooGeocoderDatabaseRunner(BackgroundWorker backgroundWorker)
            : base(backgroundWorker)
        {
            RunnerName = "Yahoo";
        }

        public YahooGeocoderDatabaseRunner(BackgroundWorker backgroundWorker, TraceSource traceSource)
            : base(backgroundWorker, traceSource)
        {
            RunnerName = "Yahoo";
        }

        public YahooGeocoderDatabaseRunner(BackgroundWorker backgroundWorker, TraceSource traceSource, DataProviderType webStatusDataProviderStatus, DatabaseType webStatusDatabaseType, string webStatusConnectionString)
            : base(backgroundWorker, traceSource, webStatusDataProviderStatus, webStatusDatabaseType, webStatusConnectionString)
        {
            RunnerName = "Yahoo";
        }

        #endregion


        public override object ProcessRecord(object recordId, object record)
        {
            GeocodeResultSet ret = new GeocodeResultSet();
            string added = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            Guid transactionGuid = Guid.NewGuid();

            try
            {
                StreetAddress streetAddress = (StreetAddress)record;
                //Address address = new Address();
                //address.City = streetAddress.City;
                //address.Country = "US";
                //address.Street = streetAddress.GetStreetAddressPortionAsString();

                BatchOptions args = (BatchOptions)BatchDatabaseOptions;

                //BaseOptions baseOptions =  new BaseOptions();
                //baseOptions.Version = args.Version;
                //baseOptions.GeocoderConfiguration = args.GeocoderConfiguration;

                ExternalYahooGeocoder yahooGeocoder = new ExternalYahooGeocoder();

                ret = yahooGeocoder.Geocode(streetAddress, args, GeocoderConfiguration);
                ret.TransactionId = transactionGuid.ToString();

            }
            catch (ThreadAbortException te)
            {
                throw te;
            }
            catch (Exception e)
            {
                if (BatchDatabaseOptions.AbortOnError)
                {
                    string currentName = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                    throw new Exception(currentName + " - Error occured processing record: " + recordId + " : " + e.Message, e);
                }
                else
                {
                    ErrorCount++;
                }
            }
            return ret;
        }

    }
}
