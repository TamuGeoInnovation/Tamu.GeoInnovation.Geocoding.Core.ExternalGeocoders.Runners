using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using USC.GISResearchLab.Common.Addresses;
using USC.GISResearchLab.Common.Core.Databases;
using USC.GISResearchLab.Common.Core.Geocoders.GeocodingQueries.Options;
using USC.GISResearchLab.Geocoding.Core.ExternalGeocoders.Google;
using USC.GISResearchLab.Geocoding.Core.OutputData;

namespace USC.GISResearchLab.Geocoding.Core.Runners.Databases
{
    public class GoogleGeocoderDatabaseRunner : NonParsedGeocoderDatabaseRunner
    {

        #region Geocoder Properties

        #endregion

        #region Constructors

        public GoogleGeocoderDatabaseRunner()
            : base() 
        {
            RunnerName = "Google";
        }

        public GoogleGeocoderDatabaseRunner(DataProviderType webStatusDataProviderStatus, DatabaseType webStatusDatabaseType, string webStatusConnectionString)
            : base(webStatusDataProviderStatus, webStatusDatabaseType, webStatusConnectionString)
        {
            RunnerName = "Google";
        }

        public GoogleGeocoderDatabaseRunner(TraceSource traceSource)
            : base(traceSource)
        {
            RunnerName = "Google";
        }

        public GoogleGeocoderDatabaseRunner(BackgroundWorker backgroundWorker)
            : base(backgroundWorker)
        {
            RunnerName = "Google";
        }

        public GoogleGeocoderDatabaseRunner(BackgroundWorker backgroundWorker, TraceSource traceSource)
            : base(backgroundWorker, traceSource)
        {
            RunnerName = "Google";
        }

        public GoogleGeocoderDatabaseRunner(BackgroundWorker backgroundWorker, TraceSource traceSource, DataProviderType webStatusDataProviderStatus, DatabaseType webStatusDatabaseType, string webStatusConnectionString)
            : base(backgroundWorker, traceSource, webStatusDataProviderStatus, webStatusDatabaseType, webStatusConnectionString)
        {
            RunnerName = "Google";
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
                ExternalGoogleGeocoder googleGeocoder = new ExternalGoogleGeocoder();
                ret = googleGeocoder.Geocode(streetAddress, args, GeocoderConfiguration);
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
