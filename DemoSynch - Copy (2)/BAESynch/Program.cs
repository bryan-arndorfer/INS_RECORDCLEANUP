//using FusionApplicationAccelerator;
using FileHelpers;
using FileHelpers.DataLink;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace BAESynch
{
    class Program
    {
        static string userName = "VendUser"; //"iuser";//"bryan.arndorfer";
        static string password = "Integration1"; //"V6~`+G7wv%&a5]L+";//"cdRW9100";
        static void Main(string[] args)
        {
            Quote_Find2();
            //Contact_RNT_cleanup();
            //CountryCSV[] csv_info = get_data_fromCountry_csv();
            // DO NOT RUN! -- CountryCleanup(csv_info);
            //Thread t = new Thread(() => { Account_OSC2RNT_cleanup(csv_info); });
            //t.Start();
            //Contact_LoginCleanup();
            //Inc_Thread_Load();
            //Account_Find();
            //Partner_Find();
            for (;;)
            {
                //Console.WriteLine("Syncrhonizing RNT <--> OSC    ------  " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString());
                //Opty();
                //Account_OSC2RNT(csv_info);
                //Account_RNT2OSC(csv_info);
                //Contact_OSC2RNT(csv_info);
                //Contact_RNT2OSC(csv_info);
                //System.Threading.Thread.Sleep(60 * 1000);

            //    if(DateTime.Now.Hour == 20 && DateTime.Now.Minute == 7 && DateTime.Now.Second >= 1 && DateTime.Now.Second <=59)
            //    {
            //        //Thread t = new Thread(() => { Agreements(); });
            //        //t.Start();
            //        Agreements();
            //    }
            //    if (DateTime.Now.Hour == 21 && DateTime.Now.Minute == 09 && DateTime.Now.Second >= 1 && DateTime.Now.Second <= 59)
            //    {
            //        //Thread t = new Thread(() => { Agreements(); });
            //        //t.Start();
            //        UpdateAccountStatusForAgreements();
            //    }
            }
            //Agreements();
            //UPDATEAccountswithRNTID();
            Console.ReadKey();
        }
        public static void Account_Find()
        {
            int size = 100;
            bool cont = true;
            int start = 0;
            for (; cont == true;)
            {
                System.ServiceModel.Channels.Binding binding2 = FusionApplicationAccelerator.FaBindingFactory.GetUsernameTokenOverSslBinding();

                EndpointAddress endpointAddress2 = new EndpointAddress(new Uri("https://adc-fap0915-crm.oracledemos.com/crmCommonSalesParties/AccountService?WSDL"));


                //FusionApplicationAcceleratorTester.PriceBook.PriceBookServiceClient client = new FusionApplicationAcceleratorTester.PriceBook.PriceBookServiceClient(binding2, endpointAddress2);
                //FusionService.SalesAccountsServiceClient client = new FusionService.SalesAccountsServiceClient(binding2,endpointAddress2);
                FusionService.AccountServiceClient client = new FusionService.AccountServiceClient(binding2, endpointAddress2);
                client.ClientCredentials.UserName.UserName = userName;
                client.ClientCredentials.UserName.Password = password;

                BindingElementCollection elements = client.Endpoint.Binding.CreateBindingElements();
                elements.Find<HttpsTransportBindingElement>().MaxReceivedMessageSize = 9999999;
                elements.Find<HttpsTransportBindingElement>().MaxBufferSize = 9999999;

                client.Endpoint.Binding = new CustomBinding(elements);


                client.Endpoint.EndpointBehaviors.Add(new EmptyElementBehavior());

                FusionService.FindCriteria FC = new FusionService.FindCriteria();
                FC.fetchStart = start;
                FC.fetchSize = size;
                FC.filter = new FusionService.ViewCriteria();
                FC.filter.group = new FusionService.ViewCriteriaRow[1];
                FC.filter.group[0] = new FusionService.ViewCriteriaRow();
                FC.filter.group[0].upperCaseCompare = false;
                FC.filter.group[0].item = new FusionService.ViewCriteriaItem[1];
                FC.filter.group[0].item[0] = new FusionService.ViewCriteriaItem();
                FC.filter.group[0].item[0].attribute = "OrganizationDEO_Synch_c";
                FC.filter.group[0].item[0].@operator = "=";
                FC.filter.group[0].item[0].Items = new object[1];
                FC.filter.group[0].item[0].Items[0] = "1";
                
                //FC.filter.group[0].item[0].Items[1] = "2015-08-05T17:23:41.710";
                FC.excludeAttribute = false;

                FusionService.FindControl Control = new FusionService.FindControl();
                Control.retrieveAllTranslations = false;

                Console.WriteLine("Getting OSC Accounts");
                FusionService.DataObjectResult output = new FusionService.DataObjectResult();
                try
                {
                    output = client.findAccount(FC, Control);
                }
                catch (Exception e)
                {
                    if (e.Message.IndexOf("timed out") >= 0)
                    {
                        output = client.findAccount(FC, Control);
                    }
                }

                //Console.ReadKey();
                if (output.Value == null)
                {
                    cont = false;
                    start = 0;
                }
                else
                {
                    Console.WriteLine("Found " + output.Value.Length + " Accounts in OSC");
                    if (output.Value.Length >= size)
                    {
                        cont = true;
                        start = start + size;
                    }
                    else
                    {
                        start = 0;
                        cont = false;
                    }
                    if (output.Value.Length > 0)
                    {

                        ////We hae work to do
                        foreach (FusionService.Account A in output.Value)
                        {
                            //Call a thread to send Updates to RN
                            //Thread t = new Thread(() => { CreateRNAccount(A, cc, 2, 0); });
                            //t.Start();
                        }

                    }
                }
            }
        }
        public static void Partner_Find()
        {
            int size = 50;
            bool cont = true;
            int start = 0;
            for (; cont == true;)
            {
                System.ServiceModel.Channels.Binding binding2 = FusionApplicationAccelerator.FaBindingFactory.GetUsernameTokenOverSslBinding();

                EndpointAddress endpointAddress2 = new EndpointAddress(new Uri("https://ccaa-test.crm.us2.oraclecloud.com/partnerCenterCorePublicModel/PartnerService?WSDL"));


                //FusionApplicationAcceleratorTester.PriceBook.PriceBookServiceClient client = new FusionApplicationAcceleratorTester.PriceBook.PriceBookServiceClient(binding2, endpointAddress2);
                //FusionService.SalesAccountsServiceClient client = new FusionService.SalesAccountsServiceClient(binding2,endpointAddress2);
                FusionPartnerService.PartnerServiceClient client = new FusionPartnerService.PartnerServiceClient(binding2, endpointAddress2);
                //FusionService.AccountServiceClient client = new FusionService.AccountServiceClient(binding2, endpointAddress2);
                client.ClientCredentials.UserName.UserName = userName;
                client.ClientCredentials.UserName.Password = password;

                BindingElementCollection elements = client.Endpoint.Binding.CreateBindingElements();
                elements.Find<HttpsTransportBindingElement>().MaxReceivedMessageSize = 9999999;
                elements.Find<HttpsTransportBindingElement>().MaxBufferSize = 9999999;

                client.Endpoint.Binding = new CustomBinding(elements);


                client.Endpoint.EndpointBehaviors.Add(new EmptyElementBehavior());

                FusionPartnerService.FindCriteria FC = new FusionPartnerService.FindCriteria();
                
                FC.fetchStart = start;
                FC.fetchSize = size;
                FC.filter = new FusionPartnerService.ViewCriteria();
                FC.filter.group = new FusionPartnerService.ViewCriteriaRow[1];
                FC.filter.group[0] = new FusionPartnerService.ViewCriteriaRow();
                FC.filter.group[0].upperCaseCompare = false;
                FC.filter.group[0].item = new FusionPartnerService.ViewCriteriaItem[1];
                FC.filter.group[0].item[0] = new FusionPartnerService.ViewCriteriaItem();
                FC.filter.group[0].item[0].attribute = "PartyId";
                FC.filter.group[0].item[0].@operator = ">";
                FC.filter.group[0].item[0].Items = new object[1];
                FC.filter.group[0].item[0].Items[0] = "1";

                //FC.filter.group[0].item[0].Items[1] = "2015-08-05T17:23:41.710";
                FC.excludeAttribute = false;

                FusionPartnerService.FindControl Control = new FusionPartnerService.FindControl();
                Control.retrieveAllTranslations = false;

                Console.WriteLine("Getting OSC Partners");
                FusionPartnerService.PartnerResult output = new FusionPartnerService.PartnerResult();
                try
                {
                    output = client.findPartner(FC, Control);
                }
                catch (Exception e)
                {
                    if (e.Message.IndexOf("timed out") >= 0)
                    {
                        output = client.findPartner(FC, Control);
                    }
                }

                //Console.ReadKey();
                if (output.Value == null)
                {
                    cont = false;
                    start = 0;
                }
                else
                {
                    Console.WriteLine("Found " + output.Value.Length + " Partners in OSC");
                    if (output.Value.Length >= size)
                    {
                        cont = true;
                        start = start + size;
                    }
                    else
                    {
                        start = 0;
                        cont = false;
                    }
                    if (output.Value.Length > 0)
                    {

                        ////We hae work to do
                        foreach (FusionPartnerService.Partner A in output.Value)
                        {
                            Console.WriteLine("Found Partner: " + A.PartyId + " - " + A.OrganizationName);
                            //Call a thread to send Updates to RN
                            //Thread t = new Thread(() => { DELETE_OSC_PARTNER(A); });
                            //t.Start();
                            DELETE_OSC_PARTNER(A);
                        }

                    }
                }
            }
            Console.ReadKey();
        }

        public static void Quote_Find()
        {
            int size = 50;
            bool cont = true;
            int start = 0;
            for (; cont == true;)
            {
                System.ServiceModel.Channels.Binding binding2 = FusionApplicationAccelerator.FaBindingFactory.GetUsernameTokenOverSslBinding();

                EndpointAddress endpointAddress2 = new EndpointAddress(new Uri("https://ccaa-test.crm.us2.oraclecloud.com:443/opptyMgmtExtensibility/SalesCustomObjectService?WSDL"));


                //FusionApplicationAcceleratorTester.PriceBook.PriceBookServiceClient client = new FusionApplicationAcceleratorTester.PriceBook.PriceBookServiceClient(binding2, endpointAddress2);
                //FusionService.SalesAccountsServiceClient client = new FusionService.SalesAccountsServiceClient(binding2,endpointAddress2);
                //FusionPartnerService.PartnerServiceClient client = new FusionPartnerService.PartnerServiceClient(binding2, endpointAddress2);
                SalesCustomObjec.SalesCustomObjectServiceClient client = new SalesCustomObjec.SalesCustomObjectServiceClient(binding2, endpointAddress2);
                //FusionService.AccountServiceClient client = new FusionService.AccountServiceClient(binding2, endpointAddress2);
                client.ClientCredentials.UserName.UserName = userName;
                client.ClientCredentials.UserName.Password = password;

                BindingElementCollection elements = client.Endpoint.Binding.CreateBindingElements();
                elements.Find<HttpsTransportBindingElement>().MaxReceivedMessageSize = 9999999;
                elements.Find<HttpsTransportBindingElement>().MaxBufferSize = 9999999;

                client.Endpoint.Binding = new CustomBinding(elements);


                client.Endpoint.EndpointBehaviors.Add(new EmptyElementBehavior());

                
                SalesCustomObjec.FindCriteria FC = new SalesCustomObjec.FindCriteria();

                FC.fetchStart = start;
                FC.fetchSize = size;
                FC.filter = new SalesCustomObjec.ViewCriteria();
                FC.filter.group = new SalesCustomObjec.ViewCriteriaRow[1];
                FC.filter.group[0] = new SalesCustomObjec.ViewCriteriaRow();
                FC.filter.group[0].upperCaseCompare = false;
                FC.filter.group[0].item = new SalesCustomObjec.ViewCriteriaItem[1];
                FC.filter.group[0].item[0] = new SalesCustomObjec.ViewCriteriaItem();
                FC.filter.group[0].item[0].attribute = "LastUpdatedBy";
                FC.filter.group[0].item[0].@operator = "=";
                FC.filter.group[0].item[0].Items = new object[1];
                FC.filter.group[0].item[0].Items[0] = "gsd.admin";

                //FC.filter.group[0].item[0].Items[1] = "2015-08-05T17:23:41.710";
                FC.excludeAttribute = false;

                SalesCustomObjec.FindControl Control = new SalesCustomObjec.FindControl();
                Control.retrieveAllTranslations = false;

                Console.WriteLine("Getting OSC Partners");
                object[] output = null;
                try
                {
                    output = client.findEntity(FC, Control, "CustomObject2_c");
                }
                catch (Exception e)
                {
                    if (e.Message.IndexOf("timed out") >= 0)
                    {
                        output = client.findEntity(FC, Control, "CustomObject2_c");
                    }
                }

                //Console.ReadKey();
                if (output == null)
                {
                    cont = false;
                    start = 0;
                }
                else
                {
                    Console.WriteLine("Found " + output.Length + " Quotes in OSC");
                    if (output.Length >= size)
                    {
                        cont = true;
                        start = start + size;
                    }
                    else
                    {
                        start = 0;
                        cont = false;
                    }
                    if (output.Length > 0)
                    {

                        ////We hae work to do
                        foreach (SalesCustomObjec.CustomObject2_c A in output)
                        {
                            Console.WriteLine("Found Quote: " + A.Id + " - " + A.RecordName);
                            //Call a thread to send Updates to RN
                            //Thread t = new Thread(() => { DELETE_OSC_PARTNER(A); });
                            //t.Start();
                            DELETE_OSC_QUOTE(A);
                        }

                    }
                }
            }
            Console.ReadKey();
        }

        public static void Quote_Find2()
        {
            int size = 50;
            bool cont = true;
            int start = 0;
            for (; cont == true;)
            {
                System.ServiceModel.Channels.Binding binding2 = FusionApplicationAccelerator.FaBindingFactory.GetUsernameTokenOverSslBinding();

                EndpointAddress endpointAddress2 = new EndpointAddress(new Uri("https://ccaa-test.crm.us2.oraclecloud.com:443/opptyMgmtExtensibility/SalesCustomObjectService?WSDL"));


                //FusionApplicationAcceleratorTester.PriceBook.PriceBookServiceClient client = new FusionApplicationAcceleratorTester.PriceBook.PriceBookServiceClient(binding2, endpointAddress2);
                //FusionService.SalesAccountsServiceClient client = new FusionService.SalesAccountsServiceClient(binding2,endpointAddress2);
                //FusionPartnerService.PartnerServiceClient client = new FusionPartnerService.PartnerServiceClient(binding2, endpointAddress2);
                SalesCustomObjec.SalesCustomObjectServiceClient client = new SalesCustomObjec.SalesCustomObjectServiceClient(binding2, endpointAddress2);
                //FusionService.AccountServiceClient client = new FusionService.AccountServiceClient(binding2, endpointAddress2);
                client.ClientCredentials.UserName.UserName = userName;
                client.ClientCredentials.UserName.Password = password;

                BindingElementCollection elements = client.Endpoint.Binding.CreateBindingElements();
                elements.Find<HttpsTransportBindingElement>().MaxReceivedMessageSize = 9999999;
                elements.Find<HttpsTransportBindingElement>().MaxBufferSize = 9999999;

                client.Endpoint.Binding = new CustomBinding(elements);


                client.Endpoint.EndpointBehaviors.Add(new EmptyElementBehavior());


                SalesCustomObjec.FindCriteria FC = new SalesCustomObjec.FindCriteria();

                FC.fetchStart = start;
                FC.fetchSize = size;
                FC.filter = new SalesCustomObjec.ViewCriteria();
                FC.filter.group = new SalesCustomObjec.ViewCriteriaRow[1];
                FC.filter.group[0] = new SalesCustomObjec.ViewCriteriaRow();
                FC.filter.group[0].upperCaseCompare = false;
                FC.filter.group[0].item = new SalesCustomObjec.ViewCriteriaItem[1];
                FC.filter.group[0].item[0] = new SalesCustomObjec.ViewCriteriaItem();
                FC.filter.group[0].item[0].attribute = "LastUpdatedBy";
                FC.filter.group[0].item[0].@operator = "=";
                FC.filter.group[0].item[0].Items = new object[1];
                FC.filter.group[0].item[0].Items[0] = "VENDUSER";

                //FC.filter.group[0].item[0].Items[1] = "2015-08-05T17:23:41.710";
                FC.excludeAttribute = false;

                SalesCustomObjec.FindControl Control = new SalesCustomObjec.FindControl();
                Control.retrieveAllTranslations = false;

                Console.WriteLine("Getting OSC Partners");
                object[] output = null;
                try
                {
                    output = client.findEntity(FC, Control, "CustomObject2_c");
                }
                catch (Exception e)
                {
                    if (e.Message.IndexOf("timed out") >= 0)
                    {
                        output = client.findEntity(FC, Control, "CustomObject2_c");
                    }
                }

                //Console.ReadKey();
                if (output == null)
                {
                    cont = false;
                    start = 0;
                }
                else
                {
                    Console.WriteLine("Found " + output.Length + " Quotes in OSC");
                    if (output.Length >= size)
                    {
                        cont = true;
                        start = start + size;
                    }
                    else
                    {
                        start = 0;
                        cont = false;
                    }
                    if (output.Length > 0)
                    {

                        ////We hae work to do
                        foreach (SalesCustomObjec.CustomObject2_c A in output)
                        {
                            Console.WriteLine("Found Quote: " + A.Id + " - " + A.RecordName);
                            //Call a thread to send Updates to RN
                            //Thread t = new Thread(() => { DELETE_OSC_PARTNER(A); });
                            //t.Start();
                            //DELETE_OSC_QUOTE(A);
                            Update_OSC_QUOTE(A);
                        }

                    }
                }
            }
            Console.ReadKey();
        }

        public static void DELETE_OSC_PARTNER(FusionPartnerService.Partner a)
        {
            
            System.ServiceModel.Channels.Binding binding2 = FusionApplicationAccelerator.FaBindingFactory.GetUsernameTokenOverSslBinding();
            EndpointAddress endpointAddress2 = new EndpointAddress(new Uri("https://ccaa-test.crm.us2.oraclecloud.com/partnerCenterCorePublicModel/PartnerService?WSDL"));
            FusionPartnerService.PartnerServiceClient client = new FusionPartnerService.PartnerServiceClient(binding2, endpointAddress2);
            
            client.ClientCredentials.UserName.UserName = userName;
            client.ClientCredentials.UserName.Password = password;
            client.Endpoint.EndpointBehaviors.Add(new EmptyElementBehavior());
                try
                {
                    client.terminatePartner(a.PartyId);

                    Console.WriteLine("Terminated Partner(" + a.PartyId + ")");

                    DELETE_OSC_Account(a);
                        
                }
                catch (Exception e)
                {
                Console.WriteLine("Error OSC(" + a.PartyId + " Name: " + a.OrganizationName + ")");
                    
                }
          

        }
        public static void DELETE_OSC_QUOTE(SalesCustomObjec.CustomObject2_c a)
        {
            
            

            System.ServiceModel.Channels.Binding binding2 = FusionApplicationAccelerator.FaBindingFactory.GetUsernameTokenOverSslBinding();
            EndpointAddress endpointAddress2 = new EndpointAddress(new Uri("https://ccaa-test.crm.us2.oraclecloud.com:443/opptyMgmtExtensibility/SalesCustomObjectService?WSDL"));
            SalesCustomObjec.SalesCustomObjectServiceClient client = new SalesCustomObjec.SalesCustomObjectServiceClient(binding2, endpointAddress2);
            

            client.ClientCredentials.UserName.UserName = userName;
            client.ClientCredentials.UserName.Password = password;
            client.Endpoint.EndpointBehaviors.Add(new EmptyElementBehavior());
            try
            {
                client.deleteEntity(a, "CustomObject2_c");

                Console.WriteLine("Deleted Quote(" + a.RecordName + ")");



            }
            catch (Exception e)
            {
                Console.WriteLine("Error OSC(" + a.RecordName + " Name: " + a.Id + ")");

            }


        }

        public static void Update_OSC_QUOTE(SalesCustomObjec.CustomObject2_c a)
        {

            if(a.AccountId_c != "" && a.AccountId_c != null)
            {
                decimal id = 0;
                decimal.TryParse(a.AccountId_c, out id);
                a.Account_Id1_c = id;
                a.Account_Id1_cSpecified = true;

                a.AccountDCL_Id_c = id;
                a.AccountDCL_Id_cSpecified = true;

            }

            if(a.OpportunityId_c != "" && a.OpportunityId_c != null)
            {
                decimal di = 0;
                decimal.TryParse(a.OpportunityId_c, out di);

                a.Opportunity_Id1_c = di;
                a.Opportunity_Id1_cSpecified = true;

                a.OpportunityDCL_Id_c = di;
                a.OpportunityDCL_Id_cSpecified = true;

            }

            System.ServiceModel.Channels.Binding binding2 = FusionApplicationAccelerator.FaBindingFactory.GetUsernameTokenOverSslBinding();
            EndpointAddress endpointAddress2 = new EndpointAddress(new Uri("https://ccaa-test.crm.us2.oraclecloud.com:443/opptyMgmtExtensibility/SalesCustomObjectService?WSDL"));
            SalesCustomObjec.SalesCustomObjectServiceClient client = new SalesCustomObjec.SalesCustomObjectServiceClient(binding2, endpointAddress2);


            client.ClientCredentials.UserName.UserName = userName;
            client.ClientCredentials.UserName.Password = password;
            client.Endpoint.EndpointBehaviors.Add(new EmptyElementBehavior());
            try
            {
                client.mergeEntity(a, "CustomObject2_c");

                Console.WriteLine("Updated Quote(" + a.RecordName + ")");



            }
            catch (Exception e)
            {
                Console.WriteLine("Error OSC(" + a.RecordName + " Name: " + a.Id + ")");

            }


        }

        public static void DELETE_OSC_Account(FusionPartnerService.Partner a)
        {
            FusionService.Account Acc = new FusionService.Account();
            Acc.PartyId = a.PartyId;
            Acc.PartyIdSpecified = true;


            System.ServiceModel.Channels.Binding binding2 = FusionApplicationAccelerator.FaBindingFactory.GetUsernameTokenOverSslBinding();
            EndpointAddress endpointAddress2 = new EndpointAddress(new Uri("https://ccaa-test.crm.us2.oraclecloud.com/crmCommonSalesParties/AccountService?WSDL"));
            FusionService.AccountServiceClient client = new FusionService.AccountServiceClient(binding2, endpointAddress2);

            client.ClientCredentials.UserName.UserName = userName;
            client.ClientCredentials.UserName.Password = password;
            client.Endpoint.EndpointBehaviors.Add(new EmptyElementBehavior());
            try
            {
                client.deleteAccount(Acc);

                Console.WriteLine("Deleted Account(" + a.PartyId + ")");



            }
            catch (Exception e)
            {
                Console.WriteLine("Error OSC(" + a.PartyId + " Name: " + a.OrganizationName + ")");

            }


        }
        public static void Account_SiteCleanup()
        {
            //Get the accounts from RNT that need to be synched

            RNTService.ClientInfoHeader clientInfoHeader = new RNTService.ClientInfoHeader();
            clientInfoHeader.AppID = "AccountLookup";

            
            RNTService.Organization O = new RNTService.Organization();
            O.CustomFields = new RNTService.GenericObject();
            O.Addresses = new RNTService.TypedAddress[2];

            RNTService.ClientInfoHeader cih = new RNTService.ClientInfoHeader();
            cih.AppID = "Bryans Update";
            List<RNTService.RNObject> LRNO = new List<RNTService.RNObject>();
            //LRNO.Add(go);
            LRNO.Add(O);
            //Console.WriteLine("Getting RNT Accounts");
            //RNTService.QueryResultData[] results = GetRightNowClient().QueryObjects(cih, querystring, LRNO.ToArray(), 10);
            //if (results == null) { }
            //else
            //{
            //    Console.WriteLine("Found " + results[0].RNObjectsResult.Length + " Accounts in RNT");
            //}
            List<RNTService.RNObject> RNO = new List<RNTService.RNObject>();
            string id = "0";
            bool should_continue = true;
            for (; should_continue == true;)
            {

                Console.WriteLine("Getting RNT Accounts");
                string querystring = "SELECT Organization from Organization WHERE ID >= "+id; ;
                RNTService.QueryResultData[] results = GetRightNowClient().QueryObjects(cih, querystring, LRNO.ToArray(), 900);
                if (results == null) { should_continue = false; }
                else
                {
                    Console.WriteLine("Found " + results[0].RNObjectsResult.Length + " Accounts in RNT");
                    if (results[0].RNObjectsResult.Length == 900) { should_continue = true; } else { should_continue = false; }

                    foreach (RNTService.Organization o in results[0].RNObjectsResult)
                    {
                        foreach (RNTService.GenericField g in o.CustomFields.GenericFields)
                        {
                            if (g.name == "CO")
                            {
                                foreach (RNTService.GenericObject go in g.DataValue.Items)
                                {
                                    foreach (RNTService.GenericField gg in go.GenericFields)
                                    {
                                        if (gg.name == "Site")
                                        {
                                            if (gg.DataValue == null) { }
                                            else
                                            {
                                                string s = (string)gg.DataValue.Items[0];
                                                int leng = s.Length;
                                                if (leng < 4)
                                                {
                                                    s = s.PadLeft(4, '0');
                                                    Console.WriteLine("old" + gg.DataValue.Items[0] + "new: " + s + "ID: " + o.ID.id);
                                                }

                                                RNTService.Organization Org = new RNTService.Organization();
                                                Org.ID = new RNTService.ID();
                                                Org.ID.id = o.ID.id;
                                                Org.ID.idSpecified = true;

                                                List<RNTService.GenericField> LGF4 = new List<RNTService.GenericField>();
                                                LGF4.Add(createGenericField("Site", RNTService.ItemsChoiceType.StringValue, s));

                                                //Create the custom objects

                                                RNTService.GenericObject customFieldsc2 = new RNTService.GenericObject();
                                                customFieldsc2.GenericFields = LGF4.ToArray();
                                                //customFieldsc.GenericFields[] = ;
                                                customFieldsc2.ObjectType = new RNTService.RNObjectType();
                                                customFieldsc2.ObjectType.TypeName = "CO";

                                                RNTService.GenericField cField2 = new RNTService.GenericField();
                                                cField2.name = "CO";
                                                cField2.dataType = RNTService.DataTypeEnum.OBJECT;
                                                cField2.dataTypeSpecified = true;
                                                cField2.DataValue = new RNTService.DataValue();
                                                cField2.DataValue.Items = new object[1];
                                                cField2.DataValue.Items[0] = customFieldsc2;
                                                cField2.DataValue.ItemsElementName = new RNTService.ItemsChoiceType[1];
                                                cField2.DataValue.ItemsElementName[0] = RNTService.ItemsChoiceType.ObjectValue;


                                                //RA.CustomFields = new RNTService.GenericObject();
                                                //RA.CustomFields.GenericFields = new RNTService.GenericField[2];


                                                Org.CustomFields = new RNTService.GenericObject();
                                                Org.CustomFields.GenericFields = new RNTService.GenericField[1];
                                                Org.CustomFields.GenericFields[0] = cField2;

                                                RNO.Add(Org);
                                            }
                                        }
                                    }

                                }

                            }



                        }
                        if (RNO.Count >= 100)
                        {
                            RNTService.UpdateProcessingOptions upo = new RNTService.UpdateProcessingOptions();
                            upo.SuppressExternalEvents = true;
                            upo.SuppressRules = true;

                            RNTService.ClientInfoHeader cih1 = new RNTService.ClientInfoHeader();
                            cih1.AppID = "BAE Site Cleanup";
                            Console.WriteLine("Send Batch to RNT");
                            GetRightNowClient().Update(cih1, RNO.ToArray(), upo);
                            RNO.Clear();
                        }
                        id = o.ID.id.ToString();
                    }
                

               

                }
            }
            if (RNO.Count >= 1)
            {
                RNTService.UpdateProcessingOptions upo = new RNTService.UpdateProcessingOptions();
                upo.SuppressExternalEvents = true;
                upo.SuppressRules = true;

                RNTService.ClientInfoHeader cih1 = new RNTService.ClientInfoHeader();
                cih1.AppID = "BAE Site Cleanup";
                Console.WriteLine("Send Final Batch to RNT");
                GetRightNowClient().Update(cih1, RNO.ToArray(), upo);
                RNO.Clear();
            }
        }
        public static void Contact_LoginCleanup()
        {
            //Get the Contacts from RNT that need to be synched

            RNTService.ClientInfoHeader clientInfoHeader = new RNTService.ClientInfoHeader();
            clientInfoHeader.AppID = "ContactLookup";


            RNTService.Contact O = new RNTService.Contact();
            O.CustomFields = new RNTService.GenericObject();
            //O.Addresses = new RNTService.TypedAddress[2];

            RNTService.ClientInfoHeader cih = new RNTService.ClientInfoHeader();
            cih.AppID = "Bryans Update";
            List<RNTService.RNObject> LRNO = new List<RNTService.RNObject>();
            //LRNO.Add(go);
            LRNO.Add(O);
            //Console.WriteLine("Getting RNT Accounts");
            //RNTService.QueryResultData[] results = GetRightNowClient().QueryObjects(cih, querystring, LRNO.ToArray(), 10);
            //if (results == null) { }
            //else
            //{
            //    Console.WriteLine("Found " + results[0].RNObjectsResult.Length + " Accounts in RNT");
            //}
            List<RNTService.RNObject> RNO = new List<RNTService.RNObject>();
            string id = "0";
            bool should_continue = true;
            for (; should_continue == true;)
            {

                Console.WriteLine("Getting RNT Contacts");
                string querystring = "SELECT Contact from Contact WHERE ID >= " + id; ;
                RNTService.QueryResultData[] results = GetRightNowClient().QueryObjects(cih, querystring, LRNO.ToArray(), 900);
                if (results == null) { should_continue = false; }
                else
                {
                    Console.WriteLine("Found " + results[0].RNObjectsResult.Length + " Accounts in RNT");
                    if (results[0].RNObjectsResult.Length == 900) { should_continue = true; } else { should_continue = false; }

                    foreach (RNTService.Contact o in results[0].RNObjectsResult)
                    {
                        if (o.Login == "" || o.Login == null) { } else
                        {
                            string s = o.Login.ToLower();
                            Console.WriteLine("Old: " + o.Login + "  New: " + s + "ID: " + o.ID.id);
                            RNTService.Contact Org = new RNTService.Contact();
                            Org.ID = new RNTService.ID();
                            Org.ID.id = o.ID.id;
                            Org.ID.idSpecified = true;
                            Org.Login = s;
                            RNO.Add(Org);


                        }
                        if (RNO.Count >= 100)
                        {
                            RNTService.UpdateProcessingOptions upo = new RNTService.UpdateProcessingOptions();
                            upo.SuppressExternalEvents = true;
                            upo.SuppressRules = true;

                            RNTService.ClientInfoHeader cih1 = new RNTService.ClientInfoHeader();
                            cih1.AppID = "BAE Site Cleanup";
                            Console.WriteLine("Send Batch to RNT");
                            GetRightNowClient().Update(cih1, RNO.ToArray(), upo);
                            RNO.Clear();
                        }
                        id = o.ID.id.ToString();
                    }




                }
            }
            if (RNO.Count >= 1)
            {
                RNTService.UpdateProcessingOptions upo = new RNTService.UpdateProcessingOptions();
                upo.SuppressExternalEvents = true;
                upo.SuppressRules = true;

                RNTService.ClientInfoHeader cih1 = new RNTService.ClientInfoHeader();
                cih1.AppID = "BAE Site Cleanup";
                Console.WriteLine("Send Final Batch to RNT");
                GetRightNowClient().Update(cih1, RNO.ToArray(), upo);
                RNO.Clear();
            }
        }
        public static void Inc_Thread_Load()
        {
            ThreadLoad[] csv_info = get_data_fromThreadLoad();

            foreach(ThreadLoad TL in csv_info)
            {
                RNTService.Incident I = new RNTService.Incident();
                I.ID = new RNTService.ID();
                int id = 0;
                int.TryParse(TL.IncidentID, out id);
                I.ID.id = id;
                I.ID.idSpecified = true;

                I.Threads = new RNTService.Thread[1];
                I.Threads[0] = new RNTService.Thread();
                I.Threads[0].action = RNTService.ActionEnum.add;
                I.Threads[0].actionSpecified = true;
                I.Threads[0].Channel = new RNTService.NamedID();
                I.Threads[0].Channel.Name = "Email";
                //I.Threads[0].ContentType = new RNTService.NamedID();
                //I.Threads[0].ContentType.ID = new RNTService.ID();
                //I.Threads[0].ContentType.ID.id = 2;
                //I.Threads[0].ContentType.ID.idSpecified = true;
                I.Threads[0].EntryType = new RNTService.NamedID();
                I.Threads[0].EntryType.ID = new RNTService.ID();
                I.Threads[0].EntryType.ID.idSpecified = true;
                I.Threads[0].EntryType.ID.id = 4;
                I.Threads[0].Text = TL.Message;

                List<RNTService.RNObject> LRNO = new List<RNTService.RNObject>();
                LRNO.Add(I);

                RNTService.ClientInfoHeader cih = new RNTService.ClientInfoHeader();
                cih.AppID = "Thread Entry";

                RNTService.UpdateProcessingOptions upo = new RNTService.UpdateProcessingOptions();
                upo.SuppressExternalEvents = true;
                upo.SuppressRules = true;

                GetRightNowClient().Update(cih, LRNO.ToArray(), upo);

            }

        }



        public static void Account_OSC2RNT(CountryCSV[] cc)
        {
            int size = 100;
            bool cont = true;
            int start = 0;
            for (; cont == true;)
            {
                System.ServiceModel.Channels.Binding binding2 = FusionApplicationAccelerator.FaBindingFactory.GetUsernameTokenOverSslBinding();

                EndpointAddress endpointAddress2 = new EndpointAddress(new Uri("https://adc-fap0915-crm.oracledemos.com/crmCommonSalesParties/AccountService?WSDL"));


                //FusionApplicationAcceleratorTester.PriceBook.PriceBookServiceClient client = new FusionApplicationAcceleratorTester.PriceBook.PriceBookServiceClient(binding2, endpointAddress2);
                //FusionService.SalesAccountsServiceClient client = new FusionService.SalesAccountsServiceClient(binding2,endpointAddress2);
                FusionService.AccountServiceClient client = new FusionService.AccountServiceClient(binding2, endpointAddress2);
                client.ClientCredentials.UserName.UserName = userName;
                client.ClientCredentials.UserName.Password = password;

                BindingElementCollection elements = client.Endpoint.Binding.CreateBindingElements();
                elements.Find<HttpsTransportBindingElement>().MaxReceivedMessageSize = 9999999;
                elements.Find<HttpsTransportBindingElement>().MaxBufferSize = 9999999;

                client.Endpoint.Binding = new CustomBinding(elements);


                client.Endpoint.EndpointBehaviors.Add(new EmptyElementBehavior());

                FusionService.FindCriteria FC = new FusionService.FindCriteria();
                FC.fetchStart = start;
                FC.fetchSize = size;
                FC.filter = new FusionService.ViewCriteria();
                FC.filter.group = new FusionService.ViewCriteriaRow[1];
                FC.filter.group[0] = new FusionService.ViewCriteriaRow();
                FC.filter.group[0].upperCaseCompare = false;
                FC.filter.group[0].item = new FusionService.ViewCriteriaItem[1];
                FC.filter.group[0].item[0] = new FusionService.ViewCriteriaItem();
                FC.filter.group[0].item[0].attribute = "OrganizationDEO_Synch_c";
                FC.filter.group[0].item[0].@operator = "=";
                FC.filter.group[0].item[0].Items = new object[1];
                FC.filter.group[0].item[0].Items[0] = "1";
                //FC.filter.group[0].item[0].Items[1] = "2015-08-05T17:23:41.710";
                FC.excludeAttribute = false;

                FusionService.FindControl Control = new FusionService.FindControl();
                Control.retrieveAllTranslations = false;

                Console.WriteLine("Getting OSC Accounts");
                FusionService.DataObjectResult output = new FusionService.DataObjectResult();
                try
                {
                    output = client.findAccount(FC, Control);
                }
                catch (Exception e)
                {
                    if (e.Message.IndexOf("timed out") >= 0)
                    {
                        output = client.findAccount(FC, Control);
                    }
                }

                //Console.ReadKey();
                if (output.Value == null)
                {
                    cont = false;
                    start = 0;
                }
                else
                {
                    Console.WriteLine("Found " + output.Value.Length + " Accounts in OSC");
                    if (output.Value.Length >= size)
                    {
                        cont = true;
                        start = start + size;
                    }
                    else
                    {
                        start = 0;
                        cont = false;
                    }
                    if (output.Value.Length > 0)
                    {

                        ////We hae work to do
                        foreach (FusionService.Account A in output.Value)
                        {
                            //Call a thread to send Updates to RN
                            Thread t = new Thread(() => { CreateRNAccount(A, cc, 2,0); });
                            t.Start();
                        }

                    }
                }
            }
        }
        public static void Account_RNT2OSC(CountryCSV[] cc)
        {
            //Get the accounts from RNT that need to be synched

            RNTService.ClientInfoHeader clientInfoHeader = new RNTService.ClientInfoHeader();
            clientInfoHeader.AppID = "AccountLookup";

            string querystring = "SELECT Organization from Organization WHERE CustomFields.c.synch = 1";
            RNTService.Organization O = new RNTService.Organization();
            O.CustomFields = new RNTService.GenericObject();
            O.Addresses = new RNTService.TypedAddress[2];

            RNTService.ClientInfoHeader cih = new RNTService.ClientInfoHeader();
            cih.AppID = "Bryans Update";
            List<RNTService.RNObject> LRNO = new List<RNTService.RNObject>();
            //LRNO.Add(go);
            LRNO.Add(O);
            Console.WriteLine("Getting RNT Accounts");
            RNTService.QueryResultData[] results = GetRightNowClient().QueryObjects(cih, querystring, LRNO.ToArray(), 9000);
            if (results == null) { }
            else
            {
                Console.WriteLine("Found " + results[0].RNObjectsResult.Length + " Accounts in RNT");
            }
            foreach (RNTService.Organization o in results[0].RNObjectsResult)
            {
                //Put this into a threading
                Thread t = new Thread(() => { CreateOSCAccount(o, cc); });
                t.Start();
            }

        }
        public static void Account_RNT2OSC_cleanup(CountryCSV[] cc)
        {
            //Get the accounts from RNT that need to be synched

            RNTService.ClientInfoHeader clientInfoHeader = new RNTService.ClientInfoHeader();
            clientInfoHeader.AppID = "AccountLookup";

            string querystring = "SELECT Organization from Organization WHERE CustomFields.c.synch = 1";
            RNTService.Organization O = new RNTService.Organization();
            O.CustomFields = new RNTService.GenericObject();
            O.Addresses = new RNTService.TypedAddress[2];

            RNTService.ClientInfoHeader cih = new RNTService.ClientInfoHeader();
            cih.AppID = "Bryans Update";
            List<RNTService.RNObject> LRNO = new List<RNTService.RNObject>();
            //LRNO.Add(go);
            LRNO.Add(O);
            Console.WriteLine("Getting RNT Accounts");
            RNTService.QueryResultData[] results = GetRightNowClient().QueryObjects(cih, querystring, LRNO.ToArray(), 9000);
            if (results == null) { }
            else
            {
                Console.WriteLine("Found " + results[0].RNObjectsResult.Length + " Accounts in RNT");
            }
            foreach (RNTService.Organization o in results[0].RNObjectsResult)
            {
                //Put this into a threading
                Thread t = new Thread(() => { CreateOSCAccount(o, cc); });
                t.Start();
            }

        }
        public static void Account_OSC2RNT_cleanup(CountryCSV[] cc)
        {
            int size = 100;
            bool cont = true;
            int start = 0;
            for (; cont == true;)
            {
                System.ServiceModel.Channels.Binding binding2 = FusionApplicationAccelerator.FaBindingFactory.GetUsernameTokenOverSslBinding();

                EndpointAddress endpointAddress2 = new EndpointAddress(new Uri("https://adc-fap0915-crm.oracledemos.com/crmCommonSalesParties/AccountService?WSDL"));


                //FusionApplicationAcceleratorTester.PriceBook.PriceBookServiceClient client = new FusionApplicationAcceleratorTester.PriceBook.PriceBookServiceClient(binding2, endpointAddress2);
                //FusionService.SalesAccountsServiceClient client = new FusionService.SalesAccountsServiceClient(binding2,endpointAddress2);
                FusionService.AccountServiceClient client = new FusionService.AccountServiceClient(binding2, endpointAddress2);
                client.ClientCredentials.UserName.UserName = userName;
                client.ClientCredentials.UserName.Password = password;

                BindingElementCollection elements = client.Endpoint.Binding.CreateBindingElements();
                elements.Find<HttpsTransportBindingElement>().MaxReceivedMessageSize = 9999999;
                elements.Find<HttpsTransportBindingElement>().MaxBufferSize = 9999999;

                client.Endpoint.Binding = new CustomBinding(elements);


                client.Endpoint.EndpointBehaviors.Add(new EmptyElementBehavior());

                FusionService.FindCriteria FC = new FusionService.FindCriteria();
                FC.fetchStart = start;
                FC.fetchSize = size;
                FC.filter = new FusionService.ViewCriteria();
                FC.filter.group = new FusionService.ViewCriteriaRow[1];
                FC.filter.group[0] = new FusionService.ViewCriteriaRow();
                FC.filter.group[0].upperCaseCompare = false;
                FC.filter.group[0].item = new FusionService.ViewCriteriaItem[1];
                FC.filter.group[0].item[0] = new FusionService.ViewCriteriaItem();
                FC.filter.group[0].item[0].attribute = "OrganizationDEO_Synch_c";
                FC.filter.group[0].item[0].@operator = "=";
                FC.filter.group[0].item[0].Items = new object[1];
                FC.filter.group[0].item[0].Items[0] = "2";
                //FC.filter.group[0].item[0].Items[1] = "2015-08-05T17:23:41.710";
                FC.excludeAttribute = false;

                FusionService.FindControl Control = new FusionService.FindControl();
                Control.retrieveAllTranslations = false;

                Console.WriteLine("Getting OSC Accounts");
                FusionService.DataObjectResult output = new FusionService.DataObjectResult();
                try
                {
                    output = client.findAccount(FC, Control);
                }
                catch (Exception e)
                {
                    if (e.Message.IndexOf("timed out") >= 0)
                    {
                        output = client.findAccount(FC, Control);
                    }
                }

                //Console.ReadKey();
                if (output.Value == null)
                {
                    cont = false;
                    start = 0;
                }
                else
                {
                    Console.WriteLine("Found " + output.Value.Length + " Accounts in OSC");
                    if (output.Value.Length >= size)
                    {
                        cont = true;
                        start = start + size;
                    }
                    else
                    {
                        start = 0;
                        cont = false;
                    }
                    if (output.Value.Length > 0)
                    {

                        ////We hae work to do
                        foreach (FusionService.Account A in output.Value)
                        {
                            //Call a thread to send Updates to RN
                            Thread t = new Thread(() => { CreateRNAccount(A, cc, 3, 4); });
                            t.Start();
                        }

                    }
                }
            }
        }
        public static void Contact_OSC2RNT(CountryCSV[] cc)
        {
            int size = 100;
            bool cont = true;
            int start = 0;
            for (; cont == true;)
            {
                System.ServiceModel.Channels.Binding binding2 = FusionApplicationAccelerator.FaBindingFactory.GetUsernameTokenOverSslBinding();

                EndpointAddress endpointAddress2 = new EndpointAddress(new Uri("https://adc-fap0915-crm.oracledemos.com/foundationParties/PersonService?WSDL"));


                //FusionApplicationAcceleratorTester.PriceBook.PriceBookServiceClient client = new FusionApplicationAcceleratorTester.PriceBook.PriceBookServiceClient(binding2, endpointAddress2);
                //FusionService.SalesAccountsServiceClient client = new FusionService.SalesAccountsServiceClient(binding2,endpointAddress2);
                FusionService_P.PersonServiceClient client = new FusionService_P.PersonServiceClient(binding2, endpointAddress2);
                client.ClientCredentials.UserName.UserName = userName;
                client.ClientCredentials.UserName.Password = password;

                BindingElementCollection elements = client.Endpoint.Binding.CreateBindingElements();
                elements.Find<HttpsTransportBindingElement>().MaxReceivedMessageSize = 9999999;
                elements.Find<HttpsTransportBindingElement>().MaxBufferSize = 9999999;

                client.Endpoint.Binding = new CustomBinding(elements);


                client.Endpoint.EndpointBehaviors.Add(new EmptyElementBehavior());

                FusionService_P.FindCriteria FC = new FusionService_P.FindCriteria();
                FC.fetchStart = start;
                FC.fetchSize = size;
                FC.filter = new FusionService_P.ViewCriteria();
                FC.filter.group = new FusionService_P.ViewCriteriaRow[1];
                FC.filter.group[0] = new FusionService_P.ViewCriteriaRow();
                FC.filter.group[0].upperCaseCompare = false;
                FC.filter.group[0].item = new FusionService_P.ViewCriteriaItem[1];
                FC.filter.group[0].item[0] = new FusionService_P.ViewCriteriaItem();
                FC.filter.group[0].item[0].attribute = "Synch_c";
                FC.filter.group[0].item[0].@operator = "=";
                FC.filter.group[0].item[0].Items = new object[1];
                FC.filter.group[0].item[0].Items[0] = "1";
                //FC.filter.group[0].item[0].Items[1] = "2015-08-05T17:23:41.710";
                FC.excludeAttribute = false;

                FusionService_P.FindControl Control = new FusionService_P.FindControl();
                Control.retrieveAllTranslations = false;

                Console.WriteLine("Getting OSC Contacts");
                List<FusionService_P.PersonProfile> LSPR = new List<FusionService_P.PersonProfile>();
                FusionService_P.PersonProfile[] output = LSPR.ToArray();
                try
                {
                    output = client.findPersonProfile(FC, Control);
                }
                catch (Exception e)
                {
                    if (e.Message.IndexOf("timed out") >= 0)
                    {
                        output = client.findPersonProfile(FC, Control);
                    }
                }

                //Console.ReadKey();
                if (output == null)
                {
                    cont = false;
                    start = 0;
                }
                else
                {
                    Console.WriteLine("Found " + output.Length + " Contacts in OSC");
                    if (output.Length >= size)
                    {
                        cont = true;
                        start = start + size;
                    }
                    else
                    {
                        start = 0;
                        cont = false;
                    }
                    if (output.Length > 0)
                    {

                        ////We hae work to do
                        foreach (FusionService_P.PersonProfile A in output)
                        {
                            //Call a thread to send Updates to RN
                            Thread t = new Thread(() => { CreateRNTContactFromOSC(A, cc, 2); });
                            t.Start();
                            
                            
                        }

                    }
                }
            }
        }
        public static void Contact_RNT2OSC(CountryCSV[] cc)
        {
            //Get the Contacts from RNT that need to be synched

            RNTService.ClientInfoHeader clientInfoHeader = new RNTService.ClientInfoHeader();
            clientInfoHeader.AppID = "AccountLookup";

            string querystring = "SELECT Contact from Contact WHERE CustomFields.c.synch = 1";
            RNTService.Contact O = new RNTService.Contact();
            O.CustomFields = new RNTService.GenericObject();
            O.Emails = new List<RNTService.Email>().ToArray();
            O.Address = new RNTService.Address();
            O.Phones = new List<RNTService.Phone>().ToArray();
            

            RNTService.ClientInfoHeader cih = new RNTService.ClientInfoHeader();
            cih.AppID = "Bryans Update";
            List<RNTService.RNObject> LRNO = new List<RNTService.RNObject>();
            //LRNO.Add(go);
            LRNO.Add(O);
            Console.WriteLine("Getting RNT Contacts");
            RNTService.QueryResultData[] results = GetRightNowClient().QueryObjects(cih, querystring, LRNO.ToArray(), 9000);
            if (results == null) { }
            else
            {
                Console.WriteLine("Found " + results[0].RNObjectsResult.Length + " Contacts in RNT");
            }
            foreach (RNTService.Contact o in results[0].RNObjectsResult)
            {
                //Put this into a threading
                Thread t = new Thread(() => { CreateOSCContact(o, cc); });
                t.Start();
            }

        }
        public static void Opty()
        {
            bool run = true;
            int start = 0;
            for (; run == true;)
            {

                System.ServiceModel.Channels.Binding binding2 = FusionApplicationAccelerator.FaBindingFactory.GetUsernameTokenOverSslBinding();

                EndpointAddress endpointAddress2 = new EndpointAddress(new Uri("https://adc-fap0915-crm.oracledemos.com/opptyMgmtOpportunities/OpportunityService?WSDL"));


                //FusionApplicationAcceleratorTester.PriceBook.PriceBookServiceClient client = new FusionApplicationAcceleratorTester.PriceBook.PriceBookServiceClient(binding2, endpointAddress2);
                //FusionService.SalesAccountsServiceClient client = new FusionService.SalesAccountsServiceClient(binding2,endpointAddress2);
                FusionService_O.OpportunityServiceClient client = new FusionService_O.OpportunityServiceClient(binding2, endpointAddress2);
                client.ClientCredentials.UserName.UserName = userName;
                client.ClientCredentials.UserName.Password = password;
                client.Endpoint.EndpointBehaviors.Add(new EmptyElementBehavior());


                BindingElementCollection elements = client.Endpoint.Binding.CreateBindingElements();
                //elements.Find<SecurityBindingElement>().IncludeTimestamp = false;
                //elements.Find<HttpsTransportBindingElement>().MaxBufferPoolSize = 524288;
                elements.Find<HttpsTransportBindingElement>().MaxReceivedMessageSize = 9999999;
                elements.Find<HttpsTransportBindingElement>().MaxBufferSize = 9999999;

                client.Endpoint.Binding = new CustomBinding(elements);

                //EmptyElementBehavior

                FusionService_O.FindCriteria FC = new FusionService_O.FindCriteria();
                FC.fetchStart = start;
                FC.fetchSize = 5;
                FC.filter = new FusionService_O.ViewCriteria();

                FC.findAttribute = new string[7] { "OptyId", "CustomerAccountId", "OptyNumber", "SalesAccountId", "Name", "ChildRevenue", "PrimaryOrganizationId" };
                FC.excludeAttribute = false;
                FC.childFindCriteria = new FusionService_O.ChildFindCriteria[1];
                FC.childFindCriteria[0] = new FusionService_O.ChildFindCriteria();
                //FC.childFindCriteria[0].childAttrName = "ChildRevenue";
                FC.childFindCriteria[0].excludeAttribute = false;
                FC.childFindCriteria[0].findAttribute = new string[2] { "ChildRevenue", "Revenue" };
                FC.filter.group = new FusionService_O.ViewCriteriaRow[1];
                FC.filter.group[0] = new FusionService_O.ViewCriteriaRow();
                FC.filter.group[0].upperCaseCompare = false;
                FC.filter.group[0].item = new FusionService_O.ViewCriteriaItem[1];
                FC.filter.group[0].item[0] = new FusionService_O.ViewCriteriaItem();
                FC.filter.group[0].item[0].attribute = "Synch_c";
                FC.filter.group[0].item[0].@operator = "=";
                FC.filter.group[0].item[0].Items = new object[1];
                FC.filter.group[0].item[0].Items[0] = "1";
                //FC.filter.group[0].item[0].Items[1] = "300000014491013";
                //FC.filter.group[0].item[0].Items[2] = "300000014491014";
                //FC.filter.group[0].item[0].Items[3] = "300000014491015";
                //FC.filter.group[0].item[0].Items[4] = "300000014494828";
                //FC.filter.group[0].item[0].Items[1] = "2015-08-05T17:23:41.710";
                FC.excludeAttribute = false;


                FusionService_O.FindControl Control = new FusionService_O.FindControl();
                Control.retrieveAllTranslations = true;

                FusionService_O.Opportunity[] output = client.findOpportunity(FC, Control);



                //Console.ReadKey();
                if (output == null)
                {
                    run = false;
                }
                else
                {
                    if (output.Length > 0)
                    {
                        Console.WriteLine("----------------Opty: Optys Found: " + output.Length + "Last OptyNum: " + output[0].OptyNumber);
                        ////We hae work to do
                        foreach (FusionService_O.Opportunity A in output)
                        {
                            //Call a thread to send Updates to RN
                            if (false == true)
                            {
                                //Agreement Already Exists...  Lets skip it
                            }
                            else
                            {
                                //Agreement doesn't exist ... Lets create it
                                Thread t = new Thread(() => { CreateRNOpty(A); 
                                });
                                t.Start();

                            }


                        }

                    }
                    start = start + 5;
                    if (output.Length >= 2) { }
                    else
                    {
                        run = false;
                    }
                }
            }
        }
        public static int GetRNT_ID(FusionService_O.Opportunity opp)
        {
            int result = 0;
            System.ServiceModel.Channels.Binding binding2 = FusionApplicationAccelerator.FaBindingFactory.GetUsernameTokenOverSslBinding();

            EndpointAddress endpointAddress2 = new EndpointAddress(new Uri("https://adc-fap0915-crm.oracledemos.com/crmCommonSalesParties/AccountService?WSDL"));


            //FusionApplicationAcceleratorTester.PriceBook.PriceBookServiceClient client = new FusionApplicationAcceleratorTester.PriceBook.PriceBookServiceClient(binding2, endpointAddress2);
            //FusionService.SalesAccountsServiceClient client = new FusionService.SalesAccountsServiceClient(binding2,endpointAddress2);
            FusionService.AccountServiceClient client = new FusionService.AccountServiceClient(binding2, endpointAddress2);
            client.ClientCredentials.UserName.UserName = userName;
            client.ClientCredentials.UserName.Password = password;

            FusionService.FindCriteria FC = new FusionService.FindCriteria();
            FC.fetchStart = 0;
            FC.fetchSize = 1;
            FC.filter = new FusionService.ViewCriteria();
            FC.filter.group = new FusionService.ViewCriteriaRow[1];
            FC.filter.group[0] = new FusionService.ViewCriteriaRow();
            FC.filter.group[0].upperCaseCompare = false;
            FC.filter.group[0].item = new FusionService.ViewCriteriaItem[1];
            FC.filter.group[0].item[0] = new FusionService.ViewCriteriaItem();
            FC.filter.group[0].item[0].attribute = "PartyId";
            FC.filter.group[0].item[0].@operator = "=";
            FC.filter.group[0].item[0].Items = new object[1];
            FC.filter.group[0].item[0].Items[0] = "300000117507699";
            //FC.filter.group[0].item[0].Items[1] = "2015-08-05T17:23:41.710";
            FC.excludeAttribute = false;

            FusionService.FindControl Control = new FusionService.FindControl();
            Control.retrieveAllTranslations = false;
            FusionService.DataObjectResult output = new FusionService.DataObjectResult();
            if (opp.SalesAccountId == null) { }
            else
            {

                output = client.findAccount(FC, Control);
            }

            //Console.ReadKey();
            if (output.Value == null) { }
            else
            {
                if (output.Value.Length > 0)
                {

                    ////We hae work to do
                    foreach (FusionService.Account A in output.Value)
                    {
                        //return the output
                        result = (int)A.PartyId;
                    }

                }
            }
            return result;

        }
        public static void CreateRNOpty(FusionService_O.Opportunity opp)
        {
            RNTService.Opportunity O = new RNTService.Opportunity();

            O.Name = opp.Name;
            O.StatusWithType = new RNTService.StatusWithType();
            O.StatusWithType.Status = new RNTService.NamedID();
            O.StatusWithType.Status.Name = "Active";
            
            
            if (opp.Revenue.HasValue == false) {

                //O.SalesRepresentativeValue.Value = 0;
                    }
            else
            {
                O.SalesRepresentativeValue = new RNTService.MonetaryValue();
                O.SalesRepresentativeValue.Value = (double)opp.Revenue.Value;
                O.SalesRepresentativeValue.ValueSpecified = true;
            }
           
            O.Organization = new RNTService.NamedID();
            O.Organization.ID = new RNTService.ID();
            int h = GetRNT_ID(opp);
            
            O.Organization.ID.id = 1;
            O.Organization.ID.idSpecified = true;

            List<RNTService.RNObject> LRNO = new List<RNTService.RNObject>();
            LRNO.Add(O);

            RNTService.CreateProcessingOptions cpo = new RNTService.CreateProcessingOptions();
            cpo.SuppressRules = true;
            cpo.SuppressExternalEvents = true;

            RNTService.ClientInfoHeader cih = new RNTService.ClientInfoHeader();
            cih.AppID = "test";

            RNTService.RNObject[] result = GetRightNowClient().Create(cih, LRNO.ToArray(), cpo);
            Console.WriteLine("Created Rightnow(" + result[0].ID.id + ") Opp From OSC(" + opp.SalesAccountId + ")");
            //Write2OSC_c(.PartyId, result[0].ID.id, 0, "");


        }
        public static void CreateRNTContactFromOSC(FusionService_P.PersonProfile pp, CountryCSV[] cc, int sync_error)
        {
            RNTService.Contact C = new RNTService.Contact();

            if (pp.PersonFirstName == "" && pp.PersonLastName == "") { } else 
            {
                C.Name = new RNTService.PersonName();
                if (pp.PersonFirstName == "") { }
                else
                {
                    C.Name.First = pp.PersonFirstName;
                }
                if (pp.PersonLastName == "") { } else
                {
                    C.Name.Last = pp.PersonLastName;
                }
            }
            if (pp.PrimaryEmailAddress == "") { } else
            {
                C.Emails = new RNTService.Email[1];
                C.Emails[0] = new RNTService.Email();
                C.Emails[0].action = RNTService.ActionEnum.add;
                C.Emails[0].actionSpecified = true;
                C.Emails[0].Address = pp.PrimaryEmailAddress;
                C.Emails[0].AddressType = new RNTService.NamedID();
                C.Emails[0].AddressType.ID = new RNTService.ID();
                C.Emails[0].AddressType.ID.id = 0;
                C.Emails[0].AddressType.ID.idSpecified = true;
            }
            if (pp.PrimaryPhoneNumber == "") { } else
            {
                C.Phones = new RNTService.Phone[1];
                C.Phones[0] = new RNTService.Phone();
                C.Phones[0].action = RNTService.ActionEnum.add;
                C.Phones[0].actionSpecified = true;
                C.Phones[0].Number = pp.PrimaryPhoneNumber;
                C.Phones[0].PhoneType = new RNTService.NamedID();
                C.Phones[0].PhoneType.ID = new RNTService.ID();
                C.Phones[0].PhoneType.ID.id = 0;
                C.Phones[0].PhoneType.ID.idSpecified = true;

            }
            if (pp.PrimaryAddressCity == "" && pp.PrimaryAddressState == "" && pp.PrimaryAddressPostalCode == "" && pp.PrimaryAddressLine1 == "") { }
            else
            {
                C.Address = new RNTService.Address();
                if(pp.PrimaryAddressCity == "") { } else
                {
                    C.Address.City = pp.PrimaryAddressCity;

                }
                if(pp.PrimaryAddressCountry == null) { } else
                {
                    foreach (CountryCSV c in cc)
                    {
                        if (c.CountryCode.Trim().ToUpper() == pp.PrimaryAddressCountry.Trim().ToUpper())
                        {
                            C.Address.Country = new RNTService.NamedID();
                            C.Address.Country.ID = new RNTService.ID();
                            int country = 0;
                            int.TryParse(c.ID, out country);
                            C.Address.Country.ID.id = country;
                            C.Address.Country.ID.idSpecified = true;
                            //RA.Addresses[0].Country.Name = c.CountryName.Trim();
                        }
                    }
                }
                if(pp.PrimaryAddressState == "") { } else
                {
                    C.Address.StateOrProvince = new RNTService.NamedID();
                    C.Address.StateOrProvince.Name = pp.PrimaryAddressState;
                }
                if(pp.PrimaryAddressPostalCode == "") { } else
                {
                    C.Address.PostalCode = pp.PrimaryAddressPostalCode;
                }
                if (pp.PrimaryAddressLine1 == "") { } else
                {
                    C.Address.Street = pp.PrimaryAddressLine1;
                }
            }

            List<RNTService.GenericField> LGF = new List<RNTService.GenericField>();
            if (pp.PartyId == 0) { }
            else
            {
                RNTService.GenericField customField = new RNTService.GenericField();
                customField.name = "partyid";
                customField.dataType = RNTService.DataTypeEnum.STRING;
                customField.dataTypeSpecified = true;
                customField.DataValue = new RNTService.DataValue();
                customField.DataValue.Items = new object[1];
                customField.DataValue.ItemsElementName = new RNTService.ItemsChoiceType[1];
                customField.DataValue.Items[0] = pp.PartyId.ToString();
                customField.DataValue.ItemsElementName[0] = RNTService.ItemsChoiceType.StringValue;

                LGF.Add(customField);
            }
            

            //Create the custom objects

            RNTService.GenericObject customFieldsc = new RNTService.GenericObject();
            customFieldsc.GenericFields = LGF.ToArray();
            //customFieldsc.GenericFields[] = ;
            customFieldsc.ObjectType = new RNTService.RNObjectType();
            customFieldsc.ObjectType.TypeName = "ContactCustomFieldsc";

            RNTService.GenericField cField = new RNTService.GenericField();
            cField.name = "c";
            cField.dataType = RNTService.DataTypeEnum.OBJECT;
            cField.dataTypeSpecified = true;
            cField.DataValue = new RNTService.DataValue();
            cField.DataValue.Items = new object[1];
            cField.DataValue.Items[0] = customFieldsc;
            cField.DataValue.ItemsElementName = new RNTService.ItemsChoiceType[1];
            cField.DataValue.ItemsElementName[0] = RNTService.ItemsChoiceType.ObjectValue;


            C.CustomFields = new RNTService.GenericObject();
            C.CustomFields.GenericFields = new RNTService.GenericField[2];
            C.CustomFields.GenericFields[0] = cField;
            C.CustomFields.ObjectType = new RNTService.RNObjectType();
            C.CustomFields.ObjectType.TypeName = "ContactCustomFields";

            List<RNTService.RNObject> LRNO = new List<RNTService.RNObject>();
            LRNO.Add(C);

            RNTService.ClientInfoHeader cih = new RNTService.ClientInfoHeader();
            cih.AppID = "BAESynch";
            //Are we updating RN or Creating a new record?
            if (pp.ServiceCloudID_c != "" || pp.ServiceCloudID_c != null || pp.ServiceCloudID_c != "0")
            {
                C.Emails[0].action = RNTService.ActionEnum.update;
                C.Phones[0].action = RNTService.ActionEnum.update;

                RNTService.UpdateProcessingOptions upo = new RNTService.UpdateProcessingOptions();
                upo.SuppressExternalEvents = true;
                upo.SuppressRules = true;

                C.ID = new RNTService.ID();
                int h = 0;
                int.TryParse(pp.ServiceCloudID_c, out h);
                C.ID.id = (long)h;
                C.ID.idSpecified = true;
                

                try
                {
                    GetRightNowClient().Update(cih, LRNO.ToArray(), upo);
                    LRNO.Clear();
                    Console.WriteLine("Updated RightNow(" + pp.ServiceCloudID_c + ") Contact From OSC(" + pp.PartyId + ")");
                    int hg = 0;
                    int.TryParse(pp.ServiceCloudID_c, out h);
                    Write2OSC_c(pp.PartyId, (long)hg, 0, "");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error RightNow(" + pp.ServiceCloudID_c + ") Contact From OSC(" + pp.PartyId + ")");
                    int hg = 0;
                    int.TryParse(pp.ServiceCloudID_c, out h);
                    Write2OSC_c(pp.PartyId, (long)hg, sync_error, Left(e.Message, 1500));
                }

            }
            else
            {
                RNTService.CreateProcessingOptions cpo = new RNTService.CreateProcessingOptions();
                cpo.SuppressRules = true;
                cpo.SuppressExternalEvents = true;

                
                try
                {
                    RNTService.RNObject[] result = GetRightNowClient().Create(cih, LRNO.ToArray(), cpo);
                    Console.WriteLine("Created RightNow(" + result[0].ID.id + ") Contact From OSC(" + pp.PartyId + ")");
                    Write2OSC_c(pp.PartyId, result[0].ID.id, 0, "");
                }
                catch (Exception e)
                {
                    Write2OSC_c(pp.PartyId, 0, sync_error, Left(e.Message, 1500));
                    Console.WriteLine("Error RightNow(" + 0 + ") Contact From OSC(" + pp.PartyId + ")");
                }

            }

        }
        public static void CreateOSCContact(RNTService.Contact o, CountryCSV[] cc)
        {
            FusionService_P.PersonProfile AU = new FusionService_P.PersonProfile();

            System.Type type2 = AU.GetType();
            PropertyInfo[] properties2 = type2.GetProperties();
            foreach (System.Reflection.PropertyInfo property in properties2)
            {
                //Console.WriteLine("Name: " + property.Name + ", Value: " + property.GetValue(O, null) + ", Type: " + property.PropertyType);

                if (property.PropertyType.ToString() == "System.String"
                    && property.Name != "CreatedBy"
                    && property.Name != "LastUpdatedBy"
                    && property.Name != "TargetPartyName"
                    && property.Name != "SalesMethod"
                    && property.Name != "SalesStage"
                    && property.Name != "PrimaryContactPartyName"
                    && property.Name != "PartyName"
                    && property.Name != "PartyName1"
                    && property.Name != "PartyType"
                    && property.Name != "OrganizationName"
                    && property.Name != "TerritoryName"
                    && property.Name != "PartnerPartyName"
                    && property.Name != "PartyUniqueName"
                    && property.Name != "OwnerName"
                    && property.Name != "PartyStatus"
                    && property.Name != "CreatedByModule"
                    && property.Name != "EffectiveLatestChange"
                    && property.Name != "PartyNumber"
                   // && property.Name != "LastUpdateLogin"
                    )
                {
                    property.SetValue(AU, "", null);
                }
            }
            
            //set the matching id
            AU.ServiceCloudID_c = o.ID.id.ToString();
            //AU.ServiceCloudID_cSpecified = true;


            //wipe out any pending changes
            AU.Synch_c = 0;
            AU.Synch_cSpecified = true;

            if(o.Name.First == "")
            {
                AU.PersonFirstName = o.Name.First;
            }
            if(o.Name.Last == "")
            {
                AU.PersonLastName = o.Name.Last;
            }

            if(o.Emails == null) { } else
            {
                if(o.Emails[0].Address == "") { } else
                {
                    //AU.PrimaryEmailAddress = o.Emails[0].Address;
                }
            }
            if(o.ID.id == 0) { } else
            {
                AU.ServiceCloudID_c = o.ID.id.ToString();
            }
            if(o.Phones == null) {  } else
            {
                if (o.Phones[0].Number == "") { } else
                {
                    //AU.PrimaryPhoneNumber = o.Phones[0].Number;
                }
            }
            if (o.Address == null) { }
            else
            {


                if (o.Address.City == "") { }
                else
                {
                    //AU.PrimaryAddressCity = o.Address.City;
                }
                if (o.Address.StateOrProvince == null) { }
                else
                {
                    if (o.Address.StateOrProvince.Name == "") { }
                    else
                    {
                        //AU.PrimaryAddressState = o.Address.StateOrProvince.Name;
                    }
                }
                if (o.Address.PostalCode == "") { }
                else
                {
                    //AU.PrimaryAddressPostalCode = o.Address.PostalCode;
                }
                if (o.Address.Street == "") { }
                else
                {
                   // AU.PrimaryAddressLine1 = o.Address.Street;
                }
                if (o.Address.Country == null) { }
                else
                {
                    if (o.Address.Country.Name == "") { }
                    else
                    {
                        foreach (CountryCSV c in cc)
                        {
                            if (o.Address.Country.ID.id.ToString() == c.ID)
                            {
                                //AU.PrimaryAddressCountry = c.CountryCode;
                            }
                        }
                    }
                }
            }
            CustomRNFields_c rn = GetAllCFFields_Contact(o.ID.id.ToString());

            System.ServiceModel.Channels.Binding binding2 = FusionApplicationAccelerator.FaBindingFactory.GetUsernameTokenOverSslBinding();
            EndpointAddress endpointAddress2 = new EndpointAddress(new Uri("https://adc-fap0915-crm.oracledemos.com/foundationParties/PersonService?WSDL"));
            FusionService_P.PersonServiceClient client = new FusionService_P.PersonServiceClient(binding2, endpointAddress2);
            client.ClientCredentials.UserName.UserName = userName;
            client.ClientCredentials.UserName.Password = password;
            client.Endpoint.EndpointBehaviors.Add(new EmptyElementBehavior());
            FusionService_P.PersonResult result;
            
            FusionService_P.Person p = new FusionService_P.Person();
            List<FusionService_P.PersonProfile> lpp = new List<FusionService_P.PersonProfile>();

            System.Type type3 = p.GetType();
            PropertyInfo[] properties3 = type3.GetProperties();
            foreach (System.Reflection.PropertyInfo property in properties3)
            {
                //Console.WriteLine("Name: " + property.Name + ", Value: " + property.GetValue(O, null) + ", Type: " + property.PropertyType);

                if (property.PropertyType.ToString() == "System.String"
                    && property.Name != "CreatedBy"
                    && property.Name != "LastUpdatedBy"
                    && property.Name != "TargetPartyName"
                    && property.Name != "SalesMethod"
                    && property.Name != "SalesStage"
                    && property.Name != "PrimaryContactPartyName"
                    && property.Name != "PartyName"
                    && property.Name != "PartyName1"
                    && property.Name != "PartyType"
                    && property.Name != "OrganizationName"
                    && property.Name != "TerritoryName"
                    && property.Name != "PartnerPartyName"
                    && property.Name != "PartyUniqueName"
                    && property.Name != "OwnerName"
                    && property.Name != "PartyStatus"
                    && property.Name != "CreatedByModule"
                    && property.Name != "EffectiveLatestChange"
                    && property.Name != "PartyNumber"
                    && property.Name != "OrigSystemReference"
                    )
                {
                    property.SetValue(p, "", null);
                }
            }
            //AU.CreatedByModule = "HZ_WS";
            //p.CreatedByModule = "HZ_WS";
            //.Status = "active";
            p.PartyUsageAssignment = new FusionService_P.PartyUsageAssignment[1];
            p.PartyUsageAssignment[0] = new FusionService_P.PartyUsageAssignment();
            p.PartyUsageAssignment[0].CreatedByModule = "HZ_WS";
            p.PartyUsageAssignment[0].PartyUsageCode = "CUSTOMER_CONTACT";
            
                   System.Type type4 = p.PartyUsageAssignment[0].GetType();
            PropertyInfo[] properties4 = type4.GetProperties();
            foreach (System.Reflection.PropertyInfo property in properties4)
            {
                //Console.WriteLine("Name: " + property.Name + ", Value: " + property.GetValue(O, null) + ", Type: " + property.PropertyType);

                if (property.PropertyType.ToString() == "System.String"
                    && property.Name != "CreatedBy"
                    && property.Name != "LastUpdatedBy"
                    && property.Name != "TargetPartyName"
                    && property.Name != "SalesMethod"
                    && property.Name != "SalesStage"
                    && property.Name != "PrimaryContactPartyName"
                    && property.Name != "PartyName"
                    && property.Name != "PartyName1"
                    && property.Name != "PartyType"
                    && property.Name != "OrganizationName"
                    && property.Name != "TerritoryName"
                    && property.Name != "PartnerPartyName"
                    && property.Name != "PartyUniqueName"
                    && property.Name != "OwnerName"
                    && property.Name != "PartyStatus"
                    )
            {
                property.SetValue(p.PartyUsageAssignment[0], "", null);
            }
        }
            //p.PartyUsageAssignment[0].OwnerTableName = "HZ_PARTIES";
            //p.PartyUsageAssignment[0].CreatedBy = "bryan.arndorfer";
            //p.CreatedBy = "bryan.arndorfer";
            
            
                
            
            if (rn.partyid == "")
            {
                //New Account
                try
                {
                    lpp.Add(AU);
                    p.PersonProfile = lpp.ToArray();
                    result = client.createPerson(p);

                    //Write the PartyID back to Service Cloud
                    foreach (FusionService_P.PersonProfile rp in result.Value[0].PersonProfile)
                    {
                        Console.WriteLine("Created OSC(" + rp.PartyId + ") Contact From RNT(" + o.ID.id + ")");
                        Write2RNT_c(rp.PartyId, o.ID.id, 0);
                    }
                    
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error OSC(" + 0 + ") Contact From RNT(" + o.ID.id + ")");
                    Write2RNT_c(0, o.ID.id, 2);
                }
            }
            else
            {
                //Existing Account
                long val;
                long.TryParse(rn.partyid, out val);
                AU.PartyId = val;
                AU.PartyIdSpecified = true;
                try
                {
                    //p.PartyUsageAssignment[0].PartyId = AU.PartyId;
                    //p.PartyUsageAssignment[0].PartyIdSpecified = true;
                    
                    lpp.Add(AU);
                    p.PersonProfile = lpp.ToArray();
                    p.PartyId = AU.PartyId;
                    p.PartyIdSpecified = true;
                    
                    result = client.updatePerson(p);
                    Console.WriteLine("Updated OSC(" + AU.PartyId + ") Contact From RNT(" + o.ID.id + ")");
                    Write2RNT_c(AU.PartyId, o.ID.id, 0);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error OSC(" + AU.PartyId + ") Contact From RNT(" + o.ID.id + ")");
                    Write2RNT_c(AU.PartyId, o.ID.id, 2);
                }


            }


            //Console.ReadKey();

        }
        public static void CreateOSCAccount(RNTService.Organization o, CountryCSV[] cc)
        {
            FusionService.Account AU = new FusionService.Account();

            System.Type type2 = AU.GetType();
            PropertyInfo[] properties2 = type2.GetProperties();
            foreach (System.Reflection.PropertyInfo property in properties2)
            {
                //Console.WriteLine("Name: " + property.Name + ", Value: " + property.GetValue(O, null) + ", Type: " + property.PropertyType);

                if (property.PropertyType.ToString() == "System.String"
                    && property.Name != "CreatedBy"
                    && property.Name != "LastUpdatedBy"
                    && property.Name != "TargetPartyName"
                    && property.Name != "SalesMethod"
                    && property.Name != "SalesStage"
                    && property.Name != "PrimaryContactPartyName"
                    && property.Name != "PartyName"
                    && property.Name != "PartyName1"
                    && property.Name != "PartyType"
                    && property.Name != "OrganizationName"
                    && property.Name != "TerritoryName"
                    && property.Name != "PartnerPartyName"
                    && property.Name != "PartyUniqueName"
                    && property.Name != "OwnerName"
                    && property.Name != "PartyStatus"
                    //&& property.Name != "ExistingCustomerFlag"
                    )
                {
                    property.SetValue(AU, "", null);
                }
            }
            //AU.ExistingCustomerFlag = true;
            //AU.PartyId = A.PartyId;
            //AU.PartyIdSpecified = true;
            
            //set the matching id
            ////AU.OrganizationDEO_ServiceCloudID_c = o.ID.id.ToString();
            ////AU.PartyNumber = "APEX" + o.ID.id.ToString();
            //////AU.OrganizationDEO_ServiceCloudID_cSpecified = true;

            //////wipe out any pending changes
            ////AU.OrganizationDEO_Synch_c = 0;
            ////AU.OrganizationDEO_Synch_cSpecified = true;

            AU.OrganizationName = o.Name;

            AU.OrganizationType = "ZCA_CUSTOMER";

            if (o.NumberOfEmployees == null) { }
            else
            {
                AU.EmployeesTotal = o.NumberOfEmployees;
            }
            if (o.SalesSettings.SalesAccount.Name == "") { }
            else
            {
                AU.OwnerName = o.SalesSettings.SalesAccount.Name;
            }
            if (o.SalesSettings.TotalRevenue == null) { }
            else
            {
                AU.CurrentFiscalYearPotentialRevenueAmount = new FusionService.AmountType();
                AU.CurrentFiscalYearPotentialRevenueAmount.currencyCode = "USD";
                decimal val;
                decimal.TryParse(AU.CurrentFiscalYearPotentialRevenueAmount.Value.ToString(), out val);
                AU.CurrentFiscalYearPotentialRevenueAmount.Value = val;
            }
            //AU.PrimaryAddress = new FusionService.PrimaryAddress();

            foreach (RNTService.TypedAddress A in o.Addresses)
            {
                if (A.AddressType.ID.id == 1)
                {
                    AU.PrimaryAddress = new FusionService.PrimaryAddress();
                    if (A.City == "") { }
                    else
                    {
                        AU.PrimaryAddress.City = A.City;
                    }
                    if (A.Country.Name == "") { }
                    else
                    {
                        foreach (CountryCSV c in cc)
                        {
                            if (c.CountryName.Trim().ToUpper() == "US")//A.Country.Name.Trim().ToUpper())
                            {
                                AU.PrimaryAddress.Country = c.CountryCode.Trim();
                            }
                        }

                    }
                    if (A.StateOrProvince.Name == "") { }
                    else
                    {
                        AU.PrimaryAddress.Province = A.StateOrProvince.Name;
                    }
                    if (A.Street == "") { }
                    else
                    {
                        AU.PrimaryAddress.AddressLine1 = A.Street;
                    }
                    if (A.PostalCode == "") { }
                    else
                    {
                        AU.PrimaryAddress.PostalCode = A.PostalCode;
                    }
                }
            }


            //Get CustomFields
            CustomRNFields rn = GetAllCFFields(o.ID.id.ToString());

            //if (rn.unique_name_alias == "") { }
            //else
            //{
            //    AU.OrganizationDEO_Alias_c = rn.unique_name_alias;

            //}
            //if (rn.export_screened_date == "") { }
            //else
            //{
            //    DateTime val;
            //    DateTime.TryParse(rn.export_screened_date, out val);
            //    AU.OrganizationDEO_ExportScreenedDate_c = val;
            //}
            //if (rn.export_screened == "") { }
            //else
            //{
            //    bool val;
            //    bool.TryParse(rn.export_screened, out val);
            //    AU.OrganizationDEO_ExportScreened_c = val;
            //}
            //if (rn.industry == "") { }
            //else
            //{
            //    AU.IndustryCode = rn.industry;
            //}
            //if (rn.site == "") { }
            //else
            //{
            //    AU.OrganizationDEO_Site_c = rn.site;
            //}
            //if (rn.InsideSalesLeadId == "") { }
            //else
            //{
            //    AU.OrganizationDEO_InsideSalesLead_c = rn.InsideSalesLeadId;
            //}
            //if (rn.Status == "") { }
            //else
            //{
            //    AU.SalesProfileStatus = rn.Status;
            //}


            //AU.CreatedBy = "crm.427";
            //AU.CreationDateSpecified = false;

            System.ServiceModel.Channels.Binding binding2 = FusionApplicationAccelerator.FaBindingFactory.GetUsernameTokenOverSslBinding();
            EndpointAddress endpointAddress2 = new EndpointAddress(new Uri("https://adc-fap0915-crm.oracledemos.com/crmCommonSalesParties/AccountService?WSDL"));
            FusionService.AccountServiceClient client = new FusionService.AccountServiceClient(binding2, endpointAddress2);
            client.ClientCredentials.UserName.UserName = userName;
            client.ClientCredentials.UserName.Password = password;
            client.Endpoint.EndpointBehaviors.Add(new EmptyElementBehavior());
            FusionService.DataObjectResult result;
            //AU.ExistingCustomerFlag = null;

            if (rn.partyid == "" || rn.partyid == "0")
            {
                //New Account
                try
                {
                    result = client.createAccount(AU);

                    //Write the PartyID back to Service Cloud
                    foreach (FusionService.Account FA in result.Value)
                    {
                        Console.WriteLine("Created OSC(" + FA.PartyId + ") Account From RNT(" + o.ID.id + ")");
                        Write2RNT(FA.PartyId, o.ID.id, 0);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error OSC(" + 0 + ") Account From RNT(" + o.ID.id + ")");
                    Write2RNT(0, o.ID.id, 2);
                }
            }
            else
            {
                //Existing Account
                long val;
                long.TryParse(rn.partyid, out val);
                AU.PartyId = val;
                AU.PartyIdSpecified = true;
                try
                {
                    result = client.updateAccount(AU);
                    Console.WriteLine("Updated OSC(" + AU.PartyId + ") Account From RNT(" + o.ID.id + ")");
                    Write2RNT(AU.PartyId, o.ID.id, 0);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Created OSC(" + AU.PartyId + ") Account From RNT(" + o.ID.id + ")");
                    Write2RNT(AU.PartyId, o.ID.id, 2);
                }


            }


            //Console.ReadKey();

        }
        public class CustomRNFields
        {
            //Std CustomFields
            public string foreign_restriction;
            public string partyid;
            public string unique_name_alias;
            public string export_screened_date;
            public string export_screened;
            public string industry;
            public string site;
            public string synch;

            //CO
            public string SalesTerritory;
            public string InsideSalesLeadId;
            public string RelationshipType;
            public string SalesTeam;
            public string SalesLead;
            public string Company;
            public string Status;
            public string Site;
            public string ShipProfile;
            public string Contacts2Orgs;


        }
        public class CustomRNFields_c
        {
            public string ID;

            //Std CustomFields
            public string partyid;

        }
        public static CustomRNFields GetAllCFFields(string RN_ID)
        {
            //Console.WriteLine("Getting Account CF");
            string queryString = "SELECT ID, CustomFields.c.partyid " +
                "FROM Organization WHERE ID = " + RN_ID;
            RNTService.ClientInfoHeader clientInfoHeader = new RNTService.ClientInfoHeader();
            clientInfoHeader.AppID = "Contact Lookup";
            RNTService.CSVTableSet csvTableSet = new RNTService.CSVTableSet();
            CustomRNFields CRF = new CustomRNFields();



            byte[] bytes;
            try
            {
                csvTableSet = GetRightNowClient().QueryCSV(clientInfoHeader, queryString, 9000, "|", false, true, out bytes);
            }
            finally
            {
            }
            RNTService.CSVTable csvTable = csvTableSet.CSVTables[0];
            int i = 0;
            int contID = 0;
            foreach (string row in csvTable.Rows)
            {

                string[] values = row.Split(new char[] { '|' });

                
                CRF.partyid = values[1];
                
            }



            //MessageBox.Show(contID.ToString());
            return CRF;

        }
        public static CustomRNFields_c GetAllCFFields_Contact(string RN_ID)
        {
            //Console.WriteLine("Getting Account CF");
            string queryString = "SELECT ID, CustomFields.c.partyid FROM Contact WHERE ID = " + RN_ID;
            RNTService.ClientInfoHeader clientInfoHeader = new RNTService.ClientInfoHeader();
            clientInfoHeader.AppID = "Contact Lookup";
            RNTService.CSVTableSet csvTableSet = new RNTService.CSVTableSet();
            CustomRNFields_c CRF = new CustomRNFields_c();



            byte[] bytes;
            try
            {
                csvTableSet = GetRightNowClient().QueryCSV(clientInfoHeader, queryString, 9000, "|", false, true, out bytes);
            }
            finally
            {
            }
            RNTService.CSVTable csvTable = csvTableSet.CSVTables[0];
            int i = 0;
            int contID = 0;
            foreach (string row in csvTable.Rows)
            {

                string[] values = row.Split(new char[] { '|' });

                CRF.ID = values[0];
                CRF.partyid = values[1];

            }



            //MessageBox.Show(contID.ToString());
            return CRF;

        }
        public static void CreateRNAccount(FusionService.Account A, CountryCSV[] cc, int sync_error, int sync_success)
        {
            List<RNTService.RNObject> LRNO_Update = new List<RNTService.RNObject>();
            List<RNTService.RNObject> LRNO_Create = new List<RNTService.RNObject>();
            RNTService.ClientInfoHeader cih = new RNTService.ClientInfoHeader();
            cih.AppID = "BAESynch";
            RNTService.Organization RA = new RNTService.Organization();

            //RA.ExternalReference

            if (A.OrganizationName == "") { }
            else
            {
                RA.Name = A.OrganizationName;
            }
            if (A.EmployeesTotal == null) { }
            else
            {
                RA.NumberOfEmployees = (int)A.EmployeesTotal;
            }
            RA.SalesSettings = new RNTService.OrganizationSalesSettings();
            //Sales Account Owner
            if (A.OwnerName == "") { }
            else
            {
                //RA.SalesSettings.SalesAccount = new RNTService.NamedIDHierarchy();
                //RA.SalesSettings.SalesAccount.Name = A.OwnerName;
            }
            if (A.CurrentFiscalYearPotentialRevenueAmount == null) { }
            else
            {
                RA.SalesSettings.TotalRevenue = new RNTService.MonetaryValue();
                RA.SalesSettings.TotalRevenue.Currency = new RNTService.NamedID();
                RA.SalesSettings.TotalRevenue.Currency.Name = "USD";
                double val;
                double.TryParse(A.CurrentFiscalYearPotentialRevenueAmount.Value.ToString(), out val);
                RA.SalesSettings.TotalRevenue.Value = val;
            }
            List<RNTService.GenericField> LGF = new List<RNTService.GenericField>();

            if (A.PartyId == 0) { }
            else
            {
                RNTService.GenericField customField = new RNTService.GenericField();
                customField.name = "partyid";
                customField.dataType = RNTService.DataTypeEnum.STRING;
                customField.dataTypeSpecified = true;
                customField.DataValue = new RNTService.DataValue();
                customField.DataValue.Items = new object[1];
                customField.DataValue.ItemsElementName = new RNTService.ItemsChoiceType[1];
                customField.DataValue.Items[0] = A.PartyId.ToString();
                customField.DataValue.ItemsElementName[0] = RNTService.ItemsChoiceType.StringValue;

                LGF.Add(customField);
            }

            //if (A.OrganizationDEO_Alias_c == "" || A.OrganizationDEO_Alias_c == null) { }
            //else
            //{
            //    RNTService.GenericField customField = new RNTService.GenericField();
            //    customField.name = "unique_name_alias";
            //    customField.dataType = RNTService.DataTypeEnum.STRING;
            //    customField.dataTypeSpecified = true;
            //    customField.DataValue = new RNTService.DataValue();
            //    customField.DataValue.Items = new object[1];
            //    customField.DataValue.ItemsElementName = new RNTService.ItemsChoiceType[1];
            //    customField.DataValue.Items[0] = A.OrganizationDEO_Alias_c;
            //    customField.DataValue.ItemsElementName[0] = RNTService.ItemsChoiceType.StringValue;

            //    LGF.Add(customField);
            //}

            //if (A.OrganizationDEO_ExportScreenedDate_c == null) { }
            //else
            //{
            //    RNTService.GenericField customField = new RNTService.GenericField();
            //    customField.name = "export_screened_date";
            //    customField.dataType = RNTService.DataTypeEnum.DATE;
            //    customField.dataTypeSpecified = true;
            //    customField.DataValue = new RNTService.DataValue();
            //    customField.DataValue.Items = new object[1];
            //    customField.DataValue.ItemsElementName = new RNTService.ItemsChoiceType[1];
            //    customField.DataValue.Items[0] = A.OrganizationDEO_ExportScreenedDate_c;
            //    customField.DataValue.ItemsElementName[0] = RNTService.ItemsChoiceType.DateValue;

            //    LGF.Add(customField);
            //}

            //if (A.OrganizationDEO_ExportScreened_c == null) { }
            //else
            //{
            //    RNTService.GenericField customField = new RNTService.GenericField();
            //    customField.name = "export_screened";
            //    customField.dataType = RNTService.DataTypeEnum.BOOLEAN;
            //    customField.dataTypeSpecified = true;
            //    customField.DataValue = new RNTService.DataValue();
            //    customField.DataValue.Items = new object[1];
            //    customField.DataValue.ItemsElementName = new RNTService.ItemsChoiceType[1];
            //    customField.DataValue.Items[0] = A.OrganizationDEO_ExportScreened_c;
            //    customField.DataValue.ItemsElementName[0] = RNTService.ItemsChoiceType.BooleanValue;

            //    LGF.Add(customField);
            //}

            //if (A.IndustryCode == null) { }
            //else
            //{
            //    RNTService.GenericField customField = new RNTService.GenericField();
            //    customField.name = "industry";
            //    customField.dataType = RNTService.DataTypeEnum.STRING;
            //    customField.dataTypeSpecified = true;
            //    customField.DataValue = new RNTService.DataValue();
            //    customField.DataValue.Items = new object[1];
            //    customField.DataValue.ItemsElementName = new RNTService.ItemsChoiceType[1];
            //    customField.DataValue.Items[0] = A.IndustryCode;
            //    customField.DataValue.ItemsElementName[0] = RNTService.ItemsChoiceType.StringValue;

            //    LGF.Add(customField);
            //}

            //if (A.OrganizationDEO_Site_c == null) { }
            //else
            //{
            //    RNTService.GenericField customField = new RNTService.GenericField();
            //    customField.name = "site";
            //    customField.dataType = RNTService.DataTypeEnum.STRING;
            //    customField.dataTypeSpecified = true;
            //    customField.DataValue = new RNTService.DataValue();
            //    customField.DataValue.Items = new object[1];
            //    customField.DataValue.ItemsElementName = new RNTService.ItemsChoiceType[1];
            //    customField.DataValue.Items[0] = A.OrganizationDEO_Site_c;
            //    customField.DataValue.ItemsElementName[0] = RNTService.ItemsChoiceType.StringValue;

            //    LGF.Add(customField);
            //}
            //if (A.Type == null) { }
            //else
            //{
            //    RNTService.GenericField customField = new RNTService.GenericField();
            //    customField.name = "type";
            //    customField.dataType = RNTService.DataTypeEnum.STRING;
            //    customField.dataTypeSpecified = true;
            //    customField.DataValue = new RNTService.DataValue();
            //    customField.DataValue.Items = new object[1];
            //    customField.DataValue.ItemsElementName = new RNTService.ItemsChoiceType[1];
            //    customField.DataValue.Items[0] = A.Type;
            //    customField.DataValue.ItemsElementName[0] = RNTService.ItemsChoiceType.StringValue;

            //    LGF.Add(customField);
            //}

            //Create the custom objects

            RNTService.GenericObject customFieldsc = new RNTService.GenericObject();
            customFieldsc.GenericFields = LGF.ToArray();
            //customFieldsc.GenericFields[] = ;
            customFieldsc.ObjectType = new RNTService.RNObjectType();
            customFieldsc.ObjectType.TypeName = "OrganizationCustomFieldsc";

            RNTService.GenericField cField = new RNTService.GenericField();
            cField.name = "c";
            cField.dataType = RNTService.DataTypeEnum.OBJECT;
            cField.dataTypeSpecified = true;
            cField.DataValue = new RNTService.DataValue();
            cField.DataValue.Items = new object[1];
            cField.DataValue.Items[0] = customFieldsc;
            cField.DataValue.ItemsElementName = new RNTService.ItemsChoiceType[1];
            cField.DataValue.ItemsElementName[0] = RNTService.ItemsChoiceType.ObjectValue;


            RA.CustomFields = new RNTService.GenericObject();
            RA.CustomFields.GenericFields = new RNTService.GenericField[2];
            RA.CustomFields.GenericFields[0] = cField;
            RA.CustomFields.ObjectType = new RNTService.RNObjectType();
            RA.CustomFields.ObjectType.TypeName = "OrganizationCustomFields";

            List<RNTService.GenericField> LGF2 = new List<RNTService.GenericField>();
            if (A.OwnerName == "") { }
            else
            {
                RNTService.GenericField customField = new RNTService.GenericField();
                customField.name = "SalesLead";
                customField.dataType = RNTService.DataTypeEnum.STRING;
                customField.dataTypeSpecified = true;
                customField.DataValue = new RNTService.DataValue();
                customField.DataValue.Items = new object[1];
                customField.DataValue.ItemsElementName = new RNTService.ItemsChoiceType[1];
                customField.DataValue.Items[0] = A.OwnerName;
                customField.DataValue.ItemsElementName[0] = RNTService.ItemsChoiceType.StringValue;

                LGF2.Add(customField);

            }

            //if (A.OrganizationDEO_InsideSalesLead_c == null) { }
            //else
            //{
            //    RNTService.NamedID val = new RNTService.NamedID();
            //    val.Name = A.OrganizationDEO_InsideSalesLead_c;

            //    RNTService.GenericField customField = new RNTService.GenericField();
            //    customField.name = "InsideSalesLeadId";
            //    customField.dataType = RNTService.DataTypeEnum.NAMED_ID;
            //    customField.dataTypeSpecified = true;
            //    customField.DataValue = new RNTService.DataValue();
            //    customField.DataValue.Items = new object[1];
            //    customField.DataValue.ItemsElementName = new RNTService.ItemsChoiceType[1];
            //    customField.DataValue.Items[0] = val;
            //    customField.DataValue.ItemsElementName[0] = RNTService.ItemsChoiceType.NamedIDValue;

            //    LGF2.Add(customField);

            //}
            //if (A.OrganizationDEO_Site_c == null) { }
            //else
            //{
            //    RNTService.GenericField customField = new RNTService.GenericField();
            //    customField.name = "Site";
            //    customField.dataType = RNTService.DataTypeEnum.STRING;
            //    customField.dataTypeSpecified = true;
            //    customField.DataValue = new RNTService.DataValue();
            //    customField.DataValue.Items = new object[1];
            //    customField.DataValue.ItemsElementName = new RNTService.ItemsChoiceType[1];
            //    customField.DataValue.Items[0] = A.OrganizationDEO_Site_c;
            //    customField.DataValue.ItemsElementName[0] = RNTService.ItemsChoiceType.StringValue;

            //    LGF2.Add(customField);

            //}

            //if (A.OrganizationDEO_Site_c == null) { }
            //else
            //{
            //    RNTService.GenericField customField = new RNTService.GenericField();
            //    customField.name = "Site";
            //    customField.dataType = RNTService.DataTypeEnum.STRING;
            //    customField.dataTypeSpecified = true;
            //    customField.DataValue = new RNTService.DataValue();
            //    customField.DataValue.Items = new object[1];
            //    customField.DataValue.ItemsElementName = new RNTService.ItemsChoiceType[1];
            //    customField.DataValue.Items[0] = A.OrganizationDEO_Site_c;
            //    customField.DataValue.ItemsElementName[0] = RNTService.ItemsChoiceType.StringValue;

            //    LGF2.Add(customField);

            //}
            //if (A.SalesProfileStatus == null) { }
            //else
            //{
            //    RNTService.GenericField customField = new RNTService.GenericField();
            //    customField.name = "Status";
            //    customField.dataType = RNTService.DataTypeEnum.STRING;
            //    customField.dataTypeSpecified = true;
            //    customField.DataValue = new RNTService.DataValue();
            //    customField.DataValue.Items = new object[1];
            //    customField.DataValue.ItemsElementName = new RNTService.ItemsChoiceType[1];
            //    customField.DataValue.Items[0] = A.SalesProfileStatus;
            //    customField.DataValue.ItemsElementName[0] = RNTService.ItemsChoiceType.StringValue;

            //    LGF2.Add(customField);
            //}

            //Create the custom objects

            //RNTService.GenericObject customFieldsc2 = new RNTService.GenericObject();
            //customFieldsc2.GenericFields = LGF2.ToArray();
            ////customFieldsc.GenericFields[] = ;
            //customFieldsc2.ObjectType = new RNTService.RNObjectType();
            //customFieldsc2.ObjectType.TypeName = "CO";

            //RNTService.GenericField cField2 = new RNTService.GenericField();
            //cField2.name = "CO";
            //cField2.dataType = RNTService.DataTypeEnum.OBJECT;
            //cField2.dataTypeSpecified = true;
            //cField2.DataValue = new RNTService.DataValue();
            //cField2.DataValue.Items = new object[1];
            //cField2.DataValue.Items[0] = customFieldsc2;
            //cField2.DataValue.ItemsElementName = new RNTService.ItemsChoiceType[1];
            //cField2.DataValue.ItemsElementName[0] = RNTService.ItemsChoiceType.ObjectValue;


            //RA.CustomFields = new RNTService.GenericObject();
            //RA.CustomFields.GenericFields = new RNTService.GenericField[2];
            //RA.CustomFields.GenericFields[1] = cField2;
            //RA.CustomFields.ObjectType = new RNTService.RNObjectType();
            //RA.CustomFields.ObjectType.TypeName = "CO";


            RA.Addresses = new RNTService.TypedAddress[1];
            RA.Addresses[0] = new RNTService.TypedAddress();
            RA.Addresses[0].AddressType = new RNTService.NamedID();
            RA.Addresses[0].AddressType.Name = "Billing";
            //Address
            if (A.PrimaryAddress == null) { }
            else
            {
                if (A.PrimaryAddress.AddressLine1 == "") { }
                else
                {
                    RA.Addresses[0].Street = A.PrimaryAddress.AddressLine1 + System.Environment.NewLine +
                        A.PrimaryAddress.AddressLine2 + System.Environment.NewLine +
                        A.PrimaryAddress.AddressLine3 + System.Environment.NewLine +
                        A.PrimaryAddress.AddressLine4;
                }
                if (A.PrimaryAddress.City == "") { }
                else
                {
                    RA.Addresses[0].City = A.PrimaryAddress.City;
                }
                if (A.PrimaryAddress.Country == "") { }
                else
                {
                    foreach (CountryCSV c in cc)
                    {
                        if (c.CountryCode.Trim().ToUpper() == A.PrimaryAddress.Country.Trim().ToUpper())
                        {
                            RA.Addresses[0].Country = new RNTService.NamedID();
                            RA.Addresses[0].Country.ID = new RNTService.ID();
                            int country = 0;
                            int.TryParse(c.ID, out country);
                            RA.Addresses[0].Country.ID.id = country;
                            RA.Addresses[0].Country.ID.idSpecified = true;
                            //RA.Addresses[0].Country.Name = c.CountryName.Trim();
                        }
                    }
                    //RA.Addresses[0].Country = new RNTService.NamedID();
                    //RA.Addresses[0].Country.Name = A.PrimaryAddress.Country;
                }
                if (A.PrimaryAddress.Province == "") { }
                else
                {
                    RA.Addresses[0].StateOrProvince = new RNTService.NamedID();
                    RA.Addresses[0].StateOrProvince.Name = A.PrimaryAddress.Province;
                }
                if (A.PrimaryAddress.PostalCode == "") { }
                else
                {
                    RA.Addresses[0].PostalCode = A.PrimaryAddress.PostalCode;
                }
            }

            //Are we updating RN or Creating a new record?
            //if (A.OrganizationDEO_ServiceCloudID_c != "" || A.OrganizationDEO_ServiceCloudID_c != null || A.OrganizationDEO_ServiceCloudID_c != "0")
            if(1==1)
            {
                RA.Addresses[0].actionSpecified = true;
                RA.Addresses[0].action = RNTService.ActionEnum.update;

                RNTService.UpdateProcessingOptions upo = new RNTService.UpdateProcessingOptions();
                upo.SuppressExternalEvents = true;
                upo.SuppressRules = true;

                RA.ID = new RNTService.ID();
                int h = 0;
                //int.TryParse(A.OrganizationDEO_ServiceCloudID_c, out h);
                RA.ID.id = (int)h;
                RA.ID.idSpecified = true;
                LRNO_Update.Add(RA);

                try
                {
                    GetRightNowClient().Update(cih, LRNO_Update.ToArray(), upo);
                    LRNO_Update.Clear();
                    ////Console.WriteLine("Updated RightNow(" + A.OrganizationDEO_ServiceCloudID_c + ") Account From OSC(" + A.PartyId + ")");
                    int hg = 0;
                    //int.TryParse(A.OrganizationDEO_ServiceCloudID_c, out hg);
                    Write2OSC(A.PartyId, (long)hg, sync_success, "");
                }
                catch (Exception e)
                {
                    ////Console.WriteLine("Error RightNow(" + A.OrganizationDEO_ServiceCloudID_c + ") Account From OSC(" + A.PartyId + ")");
                    int hg = 0;
                    //int.TryParse(A.OrganizationDEO_ServiceCloudID_c, out hg);
                    Write2OSC(A.PartyId, (long)hg, sync_error, Left(e.Message, 1500));
                }

            }
            else
            {
                RA.Addresses[0].action = RNTService.ActionEnum.add;
                RA.Addresses[0].actionSpecified = true;
                RNTService.CreateProcessingOptions cpo = new RNTService.CreateProcessingOptions();
                cpo.SuppressRules = true;
                cpo.SuppressExternalEvents = true;

                LRNO_Create.Add(RA);
                try
                {
                    RNTService.RNObject[] result = GetRightNowClient().Create(cih, LRNO_Create.ToArray(), cpo);
                    Console.WriteLine("Created RightNow(" + result[0].ID.id + ") From OSC(" + A.PartyId + ")");
                    Write2OSC(A.PartyId, result[0].ID.id, sync_success, "");
                }
                catch (Exception e)
                {
                    Write2OSC(A.PartyId, 0, sync_error, Left(e.Message, 1500));
                    Console.WriteLine("Error RightNow(" + 0 + ") Account From OSC(" + A.PartyId + ")");
                }

            }


        }
        public static RNTService.RightNowSyncPortClient _client;
        
        public static void UpdateAccountStatusForAgreements()
        {
            bool cont = true;
            long start = 0;
            for (; cont == true;)
            {
                string queryString = "SELECT Organization.ID, Organization.CustomFields.c.partyid, Organization.CustomFields.CO.Status, UE_Expire_Date FROM CO.Agreement WHERE ID >= " + start + " Order by UE_Expire_Date, ID ASC LIMIT 9000";
                RNTService.ClientInfoHeader clientInfoHeader = new RNTService.ClientInfoHeader();
                clientInfoHeader.AppID = "BAE Lookup";
                RNTService.CSVTableSet csvTableSet = new RNTService.CSVTableSet();




                byte[] bytes;
                try
                {
                    csvTableSet = GetRightNowClient().QueryCSV(clientInfoHeader, queryString, 9000, "|", false, true, out bytes);
                }
                finally
                {
                }
                RNTService.CSVTable csvTable = csvTableSet.CSVTables[0];

                if (csvTable.Rows.Length >= 9000)
                {
                    cont = true;
                    start = start + 9000;
                }
                else
                {
                    cont = false;
                }
                foreach (string row in csvTable.Rows)
                {

                    string[] values = row.Split(new char[] { '|' });
                    DateTime expire;
                    DateTime.TryParse(values[3], out expire);
                    long party;
                    long.TryParse(values[1], out party);
                    if (expire > DateTime.Now)
                    {
                        //Status Active
                        Status2OSC(party, "Active");
                    }
                    else
                    {
                        if (values[2] == "Active")
                        {
                            //Status Expired
                            Status2OSC(party, "Expired");
                        }
                    }

                }

            }


        }
        
        public static RNTService.GenericField createGenericField(string Name, RNTService.ItemsChoiceType itemsChoiceType, object Value)
        {
            RNTService.GenericField gf = new RNTService.GenericField();
            gf.name = Name; gf.DataValue = new RNTService.DataValue();
            gf.DataValue.ItemsElementName = new RNTService.ItemsChoiceType[] { itemsChoiceType };
            gf.DataValue.Items = new object[] { Value }; return gf;
        }
        public static RNTService.RightNowSyncPortClient GetRightNowClient()
        {
            if (_client == null)
            {
                //setProxy();
                //MessageBox.Show("logging in");
                string url = "https://opn-apexit-hrhd-2.rightnowdemo.com/cgi-bin/opn_apexit_hrhd_2.cfg/services/soap";
                //string url = AutomationClient.globalContext.InterfaceURL;
                //url = url.Replace("http:", "https:");
                //url = url.Replace("/php/", "/services/soap");
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                EndpointAddress endpoint = new EndpointAddress(url);
                int _sanebutusablelimit_ = 2147483647;
                BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.TransportWithMessageCredential);
                binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
                XmlDictionaryReaderQuotas myReaderQuotas = new XmlDictionaryReaderQuotas();
                myReaderQuotas.MaxStringContentLength = _sanebutusablelimit_;
                myReaderQuotas.MaxArrayLength = _sanebutusablelimit_;
                myReaderQuotas.MaxBytesPerRead = _sanebutusablelimit_;
                myReaderQuotas.MaxDepth = _sanebutusablelimit_;
                myReaderQuotas.MaxNameTableCharCount = _sanebutusablelimit_;

                binding.GetType().GetProperty("ReaderQuotas").SetValue(binding, myReaderQuotas, null);

                // Initiates _client
                RNTService.RightNowSyncPortClient client = new RNTService.RightNowSyncPortClient(binding, endpoint);
                client.ClientCredentials.UserName.UserName = userName;
                client.ClientCredentials.UserName.Password = password;

                BindingElementCollection elements = client.Endpoint.Binding.CreateBindingElements();
                elements.Find<SecurityBindingElement>().IncludeTimestamp = false;
                elements.Find<HttpsTransportBindingElement>().MaxBufferPoolSize = 524288;
                elements.Find<HttpsTransportBindingElement>().MaxReceivedMessageSize = 9999999;
                elements.Find<HttpsTransportBindingElement>().MaxBufferSize = 9999999;



                client.Endpoint.Binding = new CustomBinding(elements);
                _client = client;
                return client;
            }
            else
            {
                return _client;
            }
        }


        public class EmptyElementInspector : IClientMessageInspector
        {
            /// <summary>
            /// This will parse the response received from the service and remove all empty elements from it
            /// </summary>
            public void AfterReceiveReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
            {
                MemoryStream memoryStream = new MemoryStream();
                XmlWriter xmlWriter = XmlWriter.Create(memoryStream);
                reply.WriteMessage(xmlWriter);
                xmlWriter.Flush();
                memoryStream.Position = 0;
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(memoryStream);

                XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
                xmlNamespaceManager.AddNamespace("env", "http://schemas.xmlsoap.org/soap/envelope/");
                XmlNode header = xmlDocument.SelectSingleNode("//env:Header", xmlNamespaceManager);
                if (header != null)
                {
                    header.ParentNode.RemoveChild(header);
                }

                XmlNodeList nodes = xmlDocument.SelectNodes("//node()");
                foreach (XmlNode node in nodes)
                {

                    if (node.NodeType == XmlNodeType.Element &&
                   node.ChildNodes.Count == 0 && node.InnerXml == "" &&
                   node.Attributes.Count == 0)
                    {
                        node.ParentNode.RemoveChild(node);
                    }
                }
                memoryStream = new MemoryStream();
                xmlDocument.Save(memoryStream);
                memoryStream.Position = 0;
                XmlReader xmlReader = XmlReader.Create(memoryStream);

                System.ServiceModel.Channels.Message newMessage =
               System.ServiceModel.Channels.Message.CreateMessage(xmlReader,
               int.MaxValue, reply.Version);
                newMessage.Headers.CopyHeadersFrom(reply.Headers);
                newMessage.Properties.CopyProperties(reply.Properties);
                reply = newMessage;
            }


            public object BeforeSendRequest(ref
           System.ServiceModel.Channels.Message request,
           System.ServiceModel.IClientChannel channel)
            {
                return null;
            }
        }


        /// <summary>
        /// This class is used to add the custom inspector to process the response before de-serialization
        /// </summary>
        public class EmptyElementBehavior : System.ServiceModel.Description.IEndpointBehavior
        {

            public void
           AddBindingParameters(System.ServiceModel.Description.ServiceEndpoint
           endpoint, BindingParameterCollection bindingParameters)
            {
                //no-op
            }

            public void ApplyClientBehavior(System.ServiceModel.Description.ServiceEndpoint endpoint, ClientRuntime clientRuntime)
            {
                EmptyElementInspector inspector = new EmptyElementInspector();
                clientRuntime.MessageInspectors.Add(inspector);
            }


            public void
           ApplyDispatchBehavior(System.ServiceModel.Description.ServiceEndpoint
           endpoint, EndpointDispatcher endpointDispatcher)
            {
                //no-op
            }

            public void Validate(System.ServiceModel.Description.ServiceEndpoint endpoint)
            {
                //no-op
            }
        }

        [DelimitedRecord("|")]
        public class InitialDataPop
        {
            //[FieldQuoted('"', QuoteMode.OptionalForBoth)]
            public string AccountID;
            public string AccountName;
            public string PartyID;
            public string UniqueNameAlias;

        }

        [DelimitedRecord("|")]
        public class CountryCSV
        {
            //[FieldQuoted('"', QuoteMode.OptionalForBoth)]
            public string CountryCode;
            public string CountryName;
            public string ID;

        }


        [DelimitedRecord("|")]
        public class ThreadLoad
        {
            //[FieldQuoted('"', QuoteMode.OptionalForBoth)]
            public string IncidentID;
            public string Siebe_SRNo;
            public string SR_Row;
            public string Created;
            public string Description;
            public string Activity;
            public string Email_Recip;
            public string Email_BCC;
            public string Email_CC;
            public string Email_To;
            public string Message;
            public string Creator;

        }

        public static void Status2OSC(long PartyID, string status)
        {
            FusionService.Account AU = new FusionService.Account();

            System.Type type2 = AU.GetType();
            PropertyInfo[] properties2 = type2.GetProperties();
            foreach (System.Reflection.PropertyInfo property in properties2)
            {
                //Console.WriteLine("Name: " + property.Name + ", Value: " + property.GetValue(O, null) + ", Type: " + property.PropertyType);

                if (property.PropertyType.ToString() == "System.String"
                    && property.Name != "CreatedBy"
                    && property.Name != "LastUpdatedBy"
                    && property.Name != "TargetPartyName"
                    && property.Name != "SalesMethod"
                    && property.Name != "SalesStage"
                    && property.Name != "PrimaryContactPartyName"
                    && property.Name != "PartyName"
                    && property.Name != "PartyName1"
                    && property.Name != "PartyType"
                    && property.Name != "OrganizationName"
                    && property.Name != "TerritoryName"
                    && property.Name != "PartnerPartyName"
                    && property.Name != "PartyUniqueName"
                    && property.Name != "OwnerName"
                    && property.Name != "PartyStatus"
                    )
                {
                    property.SetValue(AU, "", null);
                }
            }
            AU.PartyUniqueName = null;

            AU.PartyId = PartyID;
            AU.PartyIdSpecified = true;

            //Update RNT
            ////AU.OrganizationDEO_Synch_c = 1;
            /////AU.OrganizationDEO_Synch_cSpecified = true;

            AU.SalesProfileStatus = status;

            System.ServiceModel.Channels.Binding binding2 = FusionApplicationAccelerator.FaBindingFactory.GetUsernameTokenOverSslBinding();
            EndpointAddress endpointAddress2 = new EndpointAddress(new Uri("https://adc-fap0915-crm.oracledemos.com/crmCommonSalesParties/AccountService?WSDL"));
            FusionService.AccountServiceClient client = new FusionService.AccountServiceClient(binding2, endpointAddress2);
            client.ClientCredentials.UserName.UserName = userName;
            client.ClientCredentials.UserName.Password = password;
            client.Endpoint.EndpointBehaviors.Add(new EmptyElementBehavior());
            try
            {
                client.updateAccount(AU);
                Console.WriteLine("Updating OSC Account(" + PartyID + ") to Status: " + status);
            }
            catch (Exception e)
            {
                if (e.InnerException.Message.IndexOf("JBO-26092: Failed to lock the record in table HZ_ORGANIZATION_PROFILES") >= 0)
                {
                    //Do nothing
                }
                else
                {
                    throw e;
                }
            }
        }
        public static InitialDataPop[] get_data_fromcsv()
        {

            ExcelStorage provider = new ExcelStorage(typeof(InitialDataPop));

            provider.StartRow = 2;
            provider.StartColumn = 1;

            string file_path = @"C:\Users\Bryan Arndorfer\Google Drive\BAE\Data\Accounts\orgs_id.csv";
            provider.FileName = file_path;

            InitialDataPop[] res = (InitialDataPop[])provider.ExtractRecords();
            return res;
        }
        public static CountryCSV[] get_data_fromCountry_csv()
        {

            ExcelStorage provider = new ExcelStorage(typeof(CountryCSV));

            provider.StartRow = 2;
            provider.StartColumn = 1;

            string file_path = @"C:\Users\Bryan Arndorfer\Documents\Visual Studio 2015\Projects\BAESynch\country_codes_excel_spreadsheet.xls";
            provider.FileName = file_path;

            CountryCSV[] res = (CountryCSV[])provider.ExtractRecords();
            return res;
        }
        public static ThreadLoad[] get_data_fromThreadLoad()
        {

            ExcelStorage provider = new ExcelStorage(typeof(ThreadLoad));

            provider.StartRow = 2;
            provider.StartColumn = 1;

            string file_path = @"C:\Users\Bryan Arndorfer\Google Drive\BAE\Data\SR\1-20\Cloud_SR_Activities_export_01232016.csv";
            provider.FileName = file_path;

            ThreadLoad[] res = (ThreadLoad[])provider.ExtractRecords();
            return res;
        }
        public static void Write2OSC(long PartyID, long RNT_ID, int sync, string error)
        {
            FusionService.Account AU = new FusionService.Account();

            System.Type type2 = AU.GetType();
            PropertyInfo[] properties2 = type2.GetProperties();
            foreach (System.Reflection.PropertyInfo property in properties2)
            {
                //Console.WriteLine("Name: " + property.Name + ", Value: " + property.GetValue(O, null) + ", Type: " + property.PropertyType);

                if (property.PropertyType.ToString() == "System.String"
                    && property.Name != "CreatedBy"
                    && property.Name != "LastUpdatedBy"
                    && property.Name != "TargetPartyName"
                    && property.Name != "SalesMethod"
                    && property.Name != "SalesStage"
                    && property.Name != "PrimaryContactPartyName"
                    && property.Name != "PartyName"
                    && property.Name != "PartyName1"
                    && property.Name != "PartyType"
                    && property.Name != "OrganizationName"
                    && property.Name != "TerritoryName"
                    && property.Name != "PartnerPartyName"
                    && property.Name != "PartyUniqueName"
                    && property.Name != "OwnerName"
                    && property.Name != "PartyStatus"
                    )
                {
                    property.SetValue(AU, "", null);
                }
            }
            AU.PartyUniqueName = null;

            AU.PartyId = PartyID;
            AU.PartyIdSpecified = true;
            //////AU.OrganizationDEO_ServiceCloudID_c = RNT_ID.ToString();
            //////AU.OrganizationDEO_ServiceCloudID_cSpecified = true;
            //////AU.OrganizationDEO_SynchError_c = error;

            //////AU.OrganizationDEO_Synch_c = sync;
            //////AU.OrganizationDEO_Synch_cSpecified = true;

            System.ServiceModel.Channels.Binding binding2 = FusionApplicationAccelerator.FaBindingFactory.GetUsernameTokenOverSslBinding();
            EndpointAddress endpointAddress2 = new EndpointAddress(new Uri("https://adc-fap0915-crm.oracledemos.com/crmCommonSalesParties/AccountService?WSDL"));
            FusionService.AccountServiceClient client = new FusionService.AccountServiceClient(binding2, endpointAddress2);
            client.ClientCredentials.UserName.UserName = userName;
            client.ClientCredentials.UserName.Password = password;
            client.Endpoint.EndpointBehaviors.Add(new EmptyElementBehavior());
            try
            {
                client.updateAccount(AU);
            }
            catch (Exception e)
            {
                if (e.InnerException.Message.IndexOf("JBO-26092: Failed to lock the record in table HZ_ORGANIZATION_PROFILES") >= 0)
                {
                    //Do nothing
                }
                else
                {
                    if (e.Message.IndexOf("timed out") >= 0)
                    {
                        //System.Threading.Thread.Sleep(10 * 1000);
                        try {
                            client.updateAccount(AU);
                        }
                        catch(Exception y)
                        {
                            throw e;
                        }
                    }
                    else
                    {
                        throw e;
                    }


                }
            }
        }
        public static void Write2OSC_c(long PartyID, long RNT_ID, int sync, string error)
        {
            FusionService_P.PersonProfile AU = new FusionService_P.PersonProfile();

            System.Type type2 = AU.GetType();
            PropertyInfo[] properties2 = type2.GetProperties();
            foreach (System.Reflection.PropertyInfo property in properties2)
            {
                //Console.WriteLine("Name: " + property.Name + ", Value: " + property.GetValue(O, null) + ", Type: " + property.PropertyType);

                if (property.PropertyType.ToString() == "System.String"
                    && property.Name != "CreatedBy"
                    && property.Name != "LastUpdatedBy"
                    && property.Name != "TargetPartyName"
                    && property.Name != "SalesMethod"
                    && property.Name != "SalesStage"
                    && property.Name != "PrimaryContactPartyName"
                    && property.Name != "PartyName"
                    && property.Name != "PartyName1"
                    && property.Name != "PartyType"
                    && property.Name != "OrganizationName"
                    && property.Name != "TerritoryName"
                    && property.Name != "PartnerPartyName"
                    && property.Name != "PartyUniqueName"
                    && property.Name != "OwnerName"
                    && property.Name != "PartyStatus"
                    && property.Name != "CreatedByModule"
                    && property.Name != "EffectiveLatestChange"
                    && property.Name != "PartyNumber"
                    )
                {
                    property.SetValue(AU, "", null);
                }
            }
            
            AU.PartyId = PartyID;
            AU.PartyIdSpecified = true;
            AU.ServiceCloudID_c = RNT_ID.ToString();
            //AU.ServiceCloudID_cSpecified = true;
            AU.SynchError_c = error;
            //AU.OrganizationDEO_SynchError_c = error;

            AU.Synch_c = sync;
            AU.Synch_cSpecified = true;

            System.ServiceModel.Channels.Binding binding2 = FusionApplicationAccelerator.FaBindingFactory.GetUsernameTokenOverSslBinding();
            EndpointAddress endpointAddress2 = new EndpointAddress(new Uri("https://adc-fap0915-crm.oracledemos.com/foundationParties/PersonService?WSDL"));
            FusionService_P.PersonServiceClient client = new FusionService_P.PersonServiceClient(binding2, endpointAddress2);
            client.ClientCredentials.UserName.UserName = userName;
            client.ClientCredentials.UserName.Password = password;
            client.Endpoint.EndpointBehaviors.Add(new EmptyElementBehavior());
            try
            {
                client.updatePersonProfile(AU);
            }
            catch (Exception e)
            {
                if (e.InnerException.Message.IndexOf("JBO-26092: Failed to lock the record in table HZ_ORGANIZATION_PROFILES") >= 0)
                {
                    //Do nothing
                }
                else
                {
                    if (e.Message.IndexOf("timed out") >= 0)
                    {
                        System.Threading.Thread.Sleep(10 * 1000);
                        client.updatePersonProfile(AU);
                    }
                    else
                    {
                        throw e;
                    }


                }
            }
        }

        public static void Write2RNT(long PartyID, long RNT_ID, int sync)
        {
            RNTService.Organization RSA = new RNTService.Organization();
            RSA.ID = new RNTService.ID();
            RSA.ID.id = RNT_ID;
            RSA.ID.idSpecified = true;

            List<RNTService.GenericField> LGF = new List<RNTService.GenericField>();

            if (PartyID == null) { }
            else
            {
                RNTService.GenericField customField = new RNTService.GenericField();
                customField.name = "partyid";
                customField.dataType = RNTService.DataTypeEnum.STRING;
                customField.dataTypeSpecified = true;
                customField.DataValue = new RNTService.DataValue();
                customField.DataValue.Items = new object[1];
                customField.DataValue.ItemsElementName = new RNTService.ItemsChoiceType[1];
                customField.DataValue.Items[0] = PartyID.ToString();
                customField.DataValue.ItemsElementName[0] = RNTService.ItemsChoiceType.StringValue;

                LGF.Add(customField);
            }

            if (PartyID == null) { }
            else
            {
                RNTService.GenericField customField = new RNTService.GenericField();
                customField.name = "synch";
                customField.dataType = RNTService.DataTypeEnum.INTEGER;
                customField.dataTypeSpecified = true;
                customField.DataValue = new RNTService.DataValue();
                customField.DataValue.Items = new object[1];
                customField.DataValue.ItemsElementName = new RNTService.ItemsChoiceType[1];
                customField.DataValue.Items[0] = sync;
                customField.DataValue.ItemsElementName[0] = RNTService.ItemsChoiceType.IntegerValue;

                LGF.Add(customField);
            }




            //Create the custom objects

            RNTService.GenericObject customFieldsc = new RNTService.GenericObject();
            customFieldsc.GenericFields = LGF.ToArray();
            //customFieldsc.GenericFields[] = ;
            customFieldsc.ObjectType = new RNTService.RNObjectType();
            customFieldsc.ObjectType.TypeName = "OrganizationCustomFieldsc";

            RNTService.GenericField cField = new RNTService.GenericField();
            cField.name = "c";
            cField.dataType = RNTService.DataTypeEnum.OBJECT;
            cField.dataTypeSpecified = true;
            cField.DataValue = new RNTService.DataValue();
            cField.DataValue.Items = new object[1];
            cField.DataValue.Items[0] = customFieldsc;
            cField.DataValue.ItemsElementName = new RNTService.ItemsChoiceType[1];
            cField.DataValue.ItemsElementName[0] = RNTService.ItemsChoiceType.ObjectValue;


            RSA.CustomFields = new RNTService.GenericObject();
            RSA.CustomFields.GenericFields = new RNTService.GenericField[2];
            RSA.CustomFields.GenericFields[0] = cField;
            RSA.CustomFields.ObjectType = new RNTService.RNObjectType();
            RSA.CustomFields.ObjectType.TypeName = "OrganizationCustomFields";

            RNTService.ClientInfoHeader cih = new RNTService.ClientInfoHeader();
            cih.AppID = "BAESynch";

            RNTService.UpdateProcessingOptions upo = new RNTService.UpdateProcessingOptions();
            upo.SuppressExternalEvents = true;
            upo.SuppressRules = true;
            List<RNTService.RNObject> LRNO_Update = new List<RNTService.RNObject>();
            LRNO_Update.Add(RSA);

            GetRightNowClient().Update(cih, LRNO_Update.ToArray(), upo);

        }
        public static void Write2RNT_c(long PartyID, long RNT_ID, int sync)
        {
            RNTService.Contact RSA = new RNTService.Contact();
            RSA.ID = new RNTService.ID();
            RSA.ID.id = RNT_ID;
            RSA.ID.idSpecified = true;

            List<RNTService.GenericField> LGF = new List<RNTService.GenericField>();

            if (PartyID == null) { }
            else
            {
                RNTService.GenericField customField = new RNTService.GenericField();
                customField.name = "partyid";
                customField.dataType = RNTService.DataTypeEnum.STRING;
                customField.dataTypeSpecified = true;
                customField.DataValue = new RNTService.DataValue();
                customField.DataValue.Items = new object[1];
                customField.DataValue.ItemsElementName = new RNTService.ItemsChoiceType[1];
                customField.DataValue.Items[0] = PartyID.ToString();
                customField.DataValue.ItemsElementName[0] = RNTService.ItemsChoiceType.StringValue;

                LGF.Add(customField);
            }

            if (PartyID == null) { }
            else
            {
                RNTService.GenericField customField = new RNTService.GenericField();
                customField.name = "synch";
                customField.dataType = RNTService.DataTypeEnum.INTEGER;
                customField.dataTypeSpecified = true;
                customField.DataValue = new RNTService.DataValue();
                customField.DataValue.Items = new object[1];
                customField.DataValue.ItemsElementName = new RNTService.ItemsChoiceType[1];
                customField.DataValue.Items[0] = sync;
                customField.DataValue.ItemsElementName[0] = RNTService.ItemsChoiceType.IntegerValue;

                LGF.Add(customField);
            }




            //Create the custom objects

            RNTService.GenericObject customFieldsc = new RNTService.GenericObject();
            customFieldsc.GenericFields = LGF.ToArray();
            //customFieldsc.GenericFields[] = ;
            customFieldsc.ObjectType = new RNTService.RNObjectType();
            customFieldsc.ObjectType.TypeName = "ContactCustomFieldsc";

            RNTService.GenericField cField = new RNTService.GenericField();
            cField.name = "c";
            cField.dataType = RNTService.DataTypeEnum.OBJECT;
            cField.dataTypeSpecified = true;
            cField.DataValue = new RNTService.DataValue();
            cField.DataValue.Items = new object[1];
            cField.DataValue.Items[0] = customFieldsc;
            cField.DataValue.ItemsElementName = new RNTService.ItemsChoiceType[1];
            cField.DataValue.ItemsElementName[0] = RNTService.ItemsChoiceType.ObjectValue;


            RSA.CustomFields = new RNTService.GenericObject();
            RSA.CustomFields.GenericFields = new RNTService.GenericField[2];
            RSA.CustomFields.GenericFields[0] = cField;
            RSA.CustomFields.ObjectType = new RNTService.RNObjectType();
            RSA.CustomFields.ObjectType.TypeName = "ContactCustomFields";

            RNTService.ClientInfoHeader cih = new RNTService.ClientInfoHeader();
            cih.AppID = "BAESynch";

            RNTService.UpdateProcessingOptions upo = new RNTService.UpdateProcessingOptions();
            upo.SuppressExternalEvents = true;
            upo.SuppressRules = true;
            List<RNTService.RNObject> LRNO_Update = new List<RNTService.RNObject>();
            LRNO_Update.Add(RSA);

            GetRightNowClient().Update(cih, LRNO_Update.ToArray(), upo);

        }
        public static void UPDATEAccountswithRNTID()
        {
            Console.WriteLine("Loading Data From File");
            InitialDataPop[] csv_info = get_data_fromcsv();
            Console.WriteLine("Done Loading Data From File");

            foreach (InitialDataPop b in csv_info)
            {
                long PartyID = 0;
                int RNTID = 0;
                long.TryParse(b.PartyID, out PartyID);
                int.TryParse(b.AccountID, out RNTID);

                if (PartyID > 0 && RNTID > 0)
                {
                    try
                    {
                        Write2OSC(PartyID, RNTID, 0, "");
                        Write2RNT(PartyID, RNTID, 0);
                        Console.WriteLine("Updated Systems---- OSC: " + PartyID + "  RNT: " + RNTID);
                    }
                    catch (Exception e)
                    {
                        try
                        {
                            Console.WriteLine("NOT Updated ---- OSC: " + PartyID + " RNT: " + RNTID);
                            Write2RNT(0, RNTID, 0);
                        }
                        catch (Exception d)
                        {
                            Write2OSC(PartyID, 0, 0, Left(d.Message, 1500));
                        }
                    }


                }
            }
        }
        public static string Left(string str, int length)
        {
            if (str == null)
                return str;
            return str.Substring(0, Math.Min(Math.Abs(length), str.Length));
        }
        public static void CountryCleanup(CountryCSV[] csv)
        {
            foreach (CountryCSV c in csv)
            {
                RNTService.Country country = new RNTService.Country();
                //country.LookupName = c.CountryName;
                country.ISOCode = c.CountryCode;
                int id = 0;
                int.TryParse(c.ID, out id);
                country.ID = new RNTService.ID();
                country.ID.id = id;
                country.ID.idSpecified = true;

                List<RNTService.RNObject> LRNO = new List<RNTService.RNObject>();
                LRNO.Add(country);

                RNTService.ClientInfoHeader cih = new RNTService.ClientInfoHeader();
                cih.AppID = "BAECountry";

                RNTService.UpdateProcessingOptions upo = new RNTService.UpdateProcessingOptions();
                upo.SuppressExternalEvents = true;
                upo.SuppressRules = true;

                if (id == 0) { }
                else
                {
                    GetRightNowClient().Update(cih, LRNO.ToArray(), upo);
                    Console.WriteLine("Updated Country: " + c.CountryName);
                }
                
            }

        }

        public static void DELETERNTAccount(RNTService.Organization o)
        {
            RNTService.Organization O = new RNTService.Organization();
            O.ID = new RNTService.ID();
            O.ID.id = o.ID.id;
            O.ID.idSpecified = true;

            RNTService.ClientInfoHeader cih = new RNTService.ClientInfoHeader();
            cih.AppID = "BAESynch";

            RNTService.UpdateProcessingOptions upo = new RNTService.UpdateProcessingOptions();
            upo.SuppressExternalEvents = true;
            upo.SuppressRules = true;

            RNTService.DestroyProcessingOptions dpo = new RNTService.DestroyProcessingOptions();
            dpo.SuppressExternalEvents = true;
            dpo.SuppressRules = true;

            List<RNTService.RNObject> LRNO_Update = new List<RNTService.RNObject>();
            LRNO_Update.Add(O);
            try
            {
                GetRightNowClient().Destroy(cih, LRNO_Update.ToArray(), dpo);
            }
            catch(Exception e)
            {
                if(e.Message.Contains("timeout"))
                {
                    try
                    {
                        GetRightNowClient().Destroy(cih, LRNO_Update.ToArray(), dpo);
                    }
                    catch (Exception d)
                    { }
                }
            }


        }
        public static void DELETERNTContact(RNTService.Contact o)
        {
            RNTService.Contact O = new RNTService.Contact();
            O.ID = new RNTService.ID();
            O.ID.id = o.ID.id;
            O.ID.idSpecified = true;

            RNTService.ClientInfoHeader cih = new RNTService.ClientInfoHeader();
            cih.AppID = "BAESynch";

            RNTService.UpdateProcessingOptions upo = new RNTService.UpdateProcessingOptions();
            upo.SuppressExternalEvents = true;
            upo.SuppressRules = true;

            RNTService.DestroyProcessingOptions dpo = new RNTService.DestroyProcessingOptions();
            dpo.SuppressExternalEvents = true;
            dpo.SuppressRules = true;

            List<RNTService.RNObject> LRNO_Update = new List<RNTService.RNObject>();
            LRNO_Update.Add(O);
            try
            {
                GetRightNowClient().Destroy(cih, LRNO_Update.ToArray(), dpo);
            }
            catch (Exception e)
            {
                if (e.Message.Contains("timeout"))
                {
                    try
                    {
                        GetRightNowClient().Destroy(cih, LRNO_Update.ToArray(), dpo);
                    }
                    catch (Exception d)
                    { }
                }
            }


        }
        public static void Account_RNT_cleanup()
        {
            //Get the accounts from RNT that need to be cleaned
            bool c = true;
            for (; c == true;)
            {
                RNTService.ClientInfoHeader clientInfoHeader = new RNTService.ClientInfoHeader();
                clientInfoHeader.AppID = "AccountLookup";

                string querystring = "SELECT Organization from Organization";
                RNTService.Organization O = new RNTService.Organization();
                O.CustomFields = new RNTService.GenericObject();
                O.Addresses = new RNTService.TypedAddress[2];

                RNTService.ClientInfoHeader cih = new RNTService.ClientInfoHeader();
                cih.AppID = "Bryans Update";
                List<RNTService.RNObject> LRNO = new List<RNTService.RNObject>();
                //LRNO.Add(go);
                LRNO.Add(O);
                Console.WriteLine("Getting RNT Accounts");
                RNTService.QueryResultData[] results = GetRightNowClient().QueryObjects(cih, querystring, LRNO.ToArray(), 1000);
                if (results == null) { c = false; }
                else
                {
                    Console.WriteLine("Found " + results[0].RNObjectsResult.Length + " Accounts in RNT");
                }
                List<Thread> LT = new List<Thread>();
                foreach (RNTService.Organization o in results[0].RNObjectsResult)
                {
                    //Put this into a threading
                    

                    Thread t = new Thread(() => { DELETERNTAccount(o); });
                    t.Start();
                    t.Name = o.ID.id.ToString();
                    LT.Add(t);
                }
                bool ct = false;
                for (;ct==false;)
                {
                    ct = true;
                    foreach(Thread T in LT)
                    {
                        if(T.IsAlive == true)
                        {
                            ct = false;
                        }
                    }

                    Console.WriteLine("Waiting...");
                    System.Threading.Thread.Sleep(5000);
                }
                
    
            }
        }
        public static void Contact_RNT_cleanup()
        {
            //Get the accounts from RNT that need to be cleaned
            bool c = true;
            for (; c == true;)
            {
                RNTService.ClientInfoHeader clientInfoHeader = new RNTService.ClientInfoHeader();
                clientInfoHeader.AppID = "ContactLookup";

                string querystring = "SELECT Contact from Contact";
                RNTService.Contact O = new RNTService.Contact();
               

                RNTService.ClientInfoHeader cih = new RNTService.ClientInfoHeader();
                cih.AppID = "Bryans Update";
                List<RNTService.RNObject> LRNO = new List<RNTService.RNObject>();
                //LRNO.Add(go);
                LRNO.Add(O);
                Console.WriteLine("Getting RNT Accounts");
                RNTService.QueryResultData[] results = GetRightNowClient().QueryObjects(cih, querystring, LRNO.ToArray(), 1000);
                if (results == null) { c = false; }
                else
                {
                    Console.WriteLine("Found " + results[0].RNObjectsResult.Length + " Contacts in RNT");
                }
                List<Thread> LT = new List<Thread>();
                foreach (RNTService.Contact o in results[0].RNObjectsResult)
                {
                    //Put this into a threading


                    Thread t = new Thread(() => { DELETERNTContact(o); });
                    t.Start();
                    t.Name = o.ID.id.ToString();
                    LT.Add(t);
                }
                bool ct = false;
                for (; ct == false;)
                {
                    ct = true;
                    foreach (Thread T in LT)
                    {
                        if (T.IsAlive == true)
                        {
                            ct = false;
                        }
                    }

                    Console.WriteLine("Waiting...");
                    System.Threading.Thread.Sleep(5000);
                }


            }
        }

    }
}
