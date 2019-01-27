using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Common.Contract;
using Microsoft.ProjectOxford.Common;
using Microsoft.ProjectOxford.Face.Contract;



namespace FaceAPIRecognition
{
    class Program
    {
        FaceServiceClient faceServiceClient = new FaceServiceClient("<YOUR API KEY HERE>", "<YOUR API ENDPOINT HERE>");
        static void Main(string[] args)
        {
        
            //Add Multiple people.
            new Program().definePeople();
            
            //Add an individual person.
            //new Program().addIndividual(/*Name of Person*/);

            //Train the Person Group.
            //new Program().trainPersonGroups();

             //Used to identify a face.
             //new Program().identifyFace(@"<Directory with image of face>");

            Console.ReadLine();
        }
        
        private async void addIndividual(String name)
        {
            CreatePersonResult friend1 = await faceServiceClient.CreatePersonAsync("myfriends", name);
            const string friend1ImageDir = @"<Directory with images of person>";
            foreach (string imagePath in Directory.GetFiles(friend1ImageDir, "*.jpg"))
            {
                using (Stream s = File.OpenRead(imagePath))
                {
                    
                    try
                    {
                        await faceServiceClient.AddPersonFaceAsync(
                            "myfriends", friend1.PersonId, s);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }

        }

        private async void identifyFace(String testImageFile)
        {
            using (Stream s = File.OpenRead(testImageFile))
            {
               
                try
                {
                    var faces = await faceServiceClient.DetectAsync(s);
                    var faceIds = faces.Select(face => face.FaceId).ToArray();

                    var results = await faceServiceClient.IdentifyAsync("myfriends", faceIds);

                    foreach (var identifyResult in results)
                    {
                        Console.WriteLine("Result of face: {0}", identifyResult.FaceId);
                        if (identifyResult.Candidates.Length == 0)
                        {
                            Console.WriteLine("No one identified");
                        }
                        else
                        {
                            // Get top 1 among all candidates returned
                            var candidateId = identifyResult.Candidates[0].PersonId;
                            var person = await faceServiceClient.GetPersonAsync("myfriends", candidateId);
                            Console.WriteLine("Identified as {0}", person.Name);
                        }
                    }
                    
                } catch (FaceAPIException e)
                {
                    Console.WriteLine("Error with identifying. " + e.Message + " " + e.ErrorCode);
                }
            }
        }

        private async void trainPersonGroups()
        {
            try
            {
                await faceServiceClient.TrainPersonGroupAsync("myfriends");
            }catch(Exception e)
            {
                Console.WriteLine("Message: " + e.Message + " Cause: " + e.Source);
            }
            TrainingStatus trainingStatus = null;
            while (true)
            {
                trainingStatus = await faceServiceClient.GetPersonGroupTrainingStatusAsync("myfriends");

                if (trainingStatus.Status != Status.Running)
                {
                    break;
                }

                await Task.Delay(1000);
            }
            Console.WriteLine("Done Training.");

        }

        private async void detectRegisterFace(CreatePersonResult friend1, CreatePersonResult friend2, CreatePersonResult friend3)
        {
           const string friend1ImageDir = @"<Directory with images of Person 1>";
            foreach (string imagePath in Directory.GetFiles(friend1ImageDir, "*.jpg"))
            {
                using (Stream s = File.OpenRead(imagePath))
                {
                    // Detect faces in the image and add them.
                    try
                    {
                        await faceServiceClient.AddPersonFaceAsync(
                            "myfriends", friend1.PersonId, s);
                    }catch(Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }

            const string friend2ImageDir = @"<Directory with images of Person 2>";
            foreach (string imagePath in Directory.GetFiles(friend2ImageDir, "*.jpg"))
            {
                using (Stream s = File.OpenRead(imagePath))
                {
                    // Detect faces in the image and add them.
                    await faceServiceClient.AddPersonFaceAsync(
                        "myfriends", friend2.PersonId, s);
                }
            }
            

            const string friend3ImageDir = @"<Directory with images of Person 3>";
            foreach (string imagePath in Directory.GetFiles(friend3ImageDir, "*.jpg"))
            {
                using (Stream s = File.OpenRead(imagePath))
                {
                    // Detect faces in the image and add.
                  await faceServiceClient.AddPersonFaceAsync(
                        "myfriends", friend3.PersonId, s);
                 
                }
            }

        }

        public async void definePeople()
        {
            string personGroupId = "myfriends";
            try
            {
                 await faceServiceClient.CreatePersonGroupAsync(personGroupId, "My Friends");
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            CreatePersonResult friend1 = await faceServiceClient.CreatePersonAsync(personGroupId, "Person 1");
            CreatePersonResult friend2 = await faceServiceClient.CreatePersonAsync(personGroupId, "Person 2");
            CreatePersonResult friend3 = await faceServiceClient.CreatePersonAsync(personGroupId, "Person 3");
            detectRegisterFace(friend1, friend2, friend3);
        }
    }
}







