using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using USC.GISResearchLab.Common.Addresses;
using USC.GISResearchLab.Common.Core.Databases;
using USC.GISResearchLab.Common.Core.Geocoders.GeocodingQueries.Options;
using USC.GISResearchLab.Geocoding.Core.ExternalGeocoders.YahooSearchScraper;
using USC.GISResearchLab.Geocoding.Core.OutputData;

namespace USC.GISResearchLab.Geocoding.Core.Runners.Databases
{
    public class YahooSearchApiGeocoderDatabaseRunner : NonParsedGeocoderDatabaseRunner
    {

        #region Geocoder Properties

        #endregion

        #region Constructors

        public YahooSearchApiGeocoderDatabaseRunner()
            : base()
        {
            RunnerName = "YahooSearchApi";
        }

        public YahooSearchApiGeocoderDatabaseRunner(DataProviderType webStatusDataProviderStatus, DatabaseType webStatusDatabaseType, string webStatusConnectionString)
            : base(webStatusDataProviderStatus, webStatusDatabaseType, webStatusConnectionString)
        {
            RunnerName = "YahooSearchApi";
        }

        public YahooSearchApiGeocoderDatabaseRunner(TraceSource traceSource)
            : base(traceSource)
        {
            RunnerName = "YahooSearchApi";
        }

        public YahooSearchApiGeocoderDatabaseRunner(BackgroundWorker backgroundWorker)
            : base(backgroundWorker)
        {
            RunnerName = "YahooSearchApi";
        }

        public YahooSearchApiGeocoderDatabaseRunner(BackgroundWorker backgroundWorker, TraceSource traceSource)
            : base(backgroundWorker, traceSource)
        {
            RunnerName = "YahooSearchApi";
        }

        public YahooSearchApiGeocoderDatabaseRunner(BackgroundWorker backgroundWorker, TraceSource traceSource, DataProviderType webStatusDataProviderStatus, DatabaseType webStatusDatabaseType, string webStatusConnectionString)
            : base(backgroundWorker, traceSource, webStatusDataProviderStatus, webStatusDatabaseType, webStatusConnectionString)
        {
            RunnerName = "YahooSearchApi";
        }

        #endregion

        public override object ProcessRecord(object recordId, object record)
        {
            return ProcessRecord(recordId, record, 2.95);
        }


        public object ProcessRecord(object recordId, object record, double version)
        {
            GeocodeResultSet ret = new GeocodeResultSet();
            string added = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            Guid transactionGuid = Guid.NewGuid();

            try
            {
                BatchOptions args = (BatchOptions)BatchDatabaseOptions;
                //BaseOptions baseOptions  = new BaseOptions();
                //baseOptions.Version = version;

                StreetAddress streetAddress = (StreetAddress)record;
                ExternalYahooSearchGeocoder YahooGeocoder = new ExternalYahooSearchGeocoder();
                ret = YahooGeocoder.Geocode(streetAddress, args, GeocoderConfiguration);
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
