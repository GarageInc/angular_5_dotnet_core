using System;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace depot {
    public interface IGoogleDriveService {
        object ListOfFiles ();
    }

    public class GoogleDriveService : IGoogleDriveService {

        // private string ApiKey = "1034645608555-in0banoa5i7pud9osmuhp5tjg81ku76o.apps.googleusercontent.com";

        static string[] Scopes = { DriveService.Scope.Drive };
        // static string ApplicationName = "PechkGoogleDriveApi";

        public object ListOfFiles () {
            UserCredential credential;

            using (var stream =
                new FileStream ("client_secret.json", FileMode.Open, FileAccess.Read)) {
                string credPath = System.Environment.GetFolderPath (
                    System.Environment.SpecialFolder.Personal);

                credPath = Path.Combine (credPath, "driveApiCredentials/drive-credentials.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync (
                    GoogleClientSecrets.Load (stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore (credPath, true)).Result;
                Console.WriteLine ("Credential file saved to: " + credPath);
            }
            // Create Drive API service.
            var service = new DriveService (new BaseClientService.Initializer () {

                HttpClientInitializer = credential,
                    // ApiKey = "AIzaSyCmjCKQ-QiVSV4Ni3KRUl7blrW5PwT5zfU",
                    ApplicationName = "PechkGoogleDriveApi",
            });

            // Define parameters of request.
            FilesResource.ListRequest listRequest = service.Files.List ();
            listRequest.PageSize = 10;
            listRequest.Fields = "nextPageToken, files(id, name)";

            // List files.
            IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute ()
                .Files;
            Console.WriteLine ("Files:");
            if (files != null && files.Count > 0) {
                foreach (var file in files) {
                    Console.WriteLine ("{0} ({1})", file.Name, file.Id);
                }
            } else {
                Console.WriteLine ("No files found.");
            }

            return new List<string> ();
        }
    }

}