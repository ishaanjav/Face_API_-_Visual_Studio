# Azure Face API - Visual Studio
**The purpose of this repository is to provide code for a Console Application in C# that uses the Microsoft Azure Face API to add people to Person Groups and then identify people given images.** Below, you can find the instructions for setting up the app and using and modifying the code for your own purposes.

-----
## Usage
###### If you have not already done so, refer to the [Setup Section](#setup) to setup the Console Application using your own Microsoft Face API Key and images of people. 
Using the application is rather simple once you have it setup. Simply run each of the functions to do different things:
1. `definePeple()`: Use this to create a `CreatePersonResult` object for each person. Inside, `definePeople`, the function `detectRegisterFace()` is called to **add images for each face in an image and add it to the corresponding person**. **IMPORTANT:** It is in `detectRegisterFace()`, lines 122, 139, and 151 that you must specify the directory of a folder contain `jpg` images of each person. 
2. `trainPersonGroup()` is called to **train the Person Group** on the people and their images. There are no parameters or specific directories required for this function.
3. `identifyFace()` is called to **identify a face**. It accepts **one parameter: the directory containing an image of a person**. It then gets the best match of the person in the image and prints their name in the Console. If it is not able to identify the person in the image, then it doesn't display anything.


-----

## Setup 
Microsoft Azure has documentation on [Detecting Faces](https://docs.microsoft.com/en-us/azure/cognitive-services/face/quickstarts/csharp-detect-sdk) and [Identifying Faces](https://docs.microsoft.com/en-us/azure/cognitive-services/face/face-api-how-to-topics/howtoidentifyfacesinimage) from images. Below are the steps to setup the Console App from the `Program.cs` file in this repository.
### 1. Create the Visual Studio Project
  * In Visual Studio create a **new Console App** from the .Net Framework. 
  * Get the required **NuGet Packages** - 
     * In the Solution Explorer, right-click on your project. 
     * Select "Manage NuGet Packages"
     * Click the "Browse" tab and select "Include prerelease"
     * Find and install this package or a similar package, *(if there has been an update)*: `Microsoft.Azure.CognitiveServices.Vision.Face 2.2.0-preview`
  * After creating the project, open the `.cs` file and paste the code from this repository's [`Program.cs`](https://github.com/ishaanjav/Azure_Face_API_-_Visual_Studio/blob/master/Program.cs) into your `.cs` file. Then, follow the steps below to get the [Microsoft API Key](#2.-get-the-microsoft-face-api), Add the API Key and Endpoint, and Specify the Directory of the Images.
     
### 2. Get the Microsoft Face API
  * **Making the Azure Account:**
In order to use the Face API, you must get an API Subscription Key from the Azure Portal. [This page](https://azure.microsoft.com/en-us/services/cognitive-services/face/) by Microsoft provides the features and capabilities of the Face API. **You can create a free Azure account that doesn't expire at [this link here](https://azure.microsoft.com/en-us/try/cognitive-services/?api=face-api) by clicking on the "Get API Key" button and choosing the option to create an Azure account**. 
  * **Getting the Face API Key from Azure Portal:**
     * Once you have created your account, head to the [Azure Portal](https://portal.azure.com/#home). Follow these steps:
        1. Click on **"Create a resource"** on the left side of the portal.
        2. Underneath **"Azure Marketplace"**, click on the **"AI + Machine Learning"** section. 
        3. Now, under **"Featured"** you should see **"Face"**. Click on that.
        4. You should now be at [this page](https://portal.azure.com/#create/Microsoft.CognitiveServicesFace). **Fill in the required information and press "Create" when done**.
        5. Now, click on **"All resources"** on the left hand side of the Portal.
        6. Click on the **name you gave the API**.
        7. Underneath **"Resource Management"**, click on **"Manage Keys"**.

<p align="center">
  <img width="900" src="https://github.com/ishaanjav/Face_Analyzer/blob/master/Azure-FaceAPI%20Key.PNG">
</p>

You should now be able to see two different subscription keys that you can use. Follow the additional instructions to see how to use the API Key in the application.

### 3. Add the API Key and Endpoint
In `Program.cs`, on Line 17 you may see:

    FaceServiceClient faceServiceClient = new FaceServiceClient("<YOUR API KEY HERE>", "<YOUR API ENDPOINT HERE>");
`<YOUR API KEY HERE>` should be replaced with the [API Key](#get-the-microsoft-face-api) that you got from the [Azure Portal](https://portal.azure.com/#home). `<YOUR ENDPOINT HERE>` should be replaced with one of the following examples from [this API Documentation link](https://westus.dev.cognitive.microsoft.com/docs/services/563879b61984550e40cbbe8d/operations/563879b61984550f30395236). The format should be similar to: 
  
    "https://<LOCATION>/face/v1.0"
  
where `<LOCATION>` should be replaced with something like `uksouth.api.cognitive.microsoft.com` or `japaneast.api.cognitive.microsoft.com`. All of these can be found, listed at [this link](https://westus.dev.cognitive.microsoft.com/docs/services/563879b61984550e40cbbe8d/operations/563879b61984550f30395236).

### 4. Specify the Directories of the Images
Now that you have gotten your API Key and Endpoint and used them as the constructors of `FaceServiceClient` on Line 17, you have to **specify the directories of the images for the people that will be identified.** The `identifyFace()` function accepts a String as a parameter with the directory of an image containing a face, but the other functions, *such as `addIndividual` and `definePeople`*, require you to change the directory in the function itself. 

Where it says:

    @"<Directory with images of person>"   //Line 40.
    //OR:
    @"<Directory with images of Person #>"   //Lines 122, 139, 151.
replace the `<Directory with images of person>` with the directory of an images folder on your computer. Images have been provided in this repository which you can download to test the Console Application. Once you have replaced `<Directory with images of Person 1` with the actual directories of images, you can run the functions one by one, *starting with `definePeople()`* to:
- add people to Person Groups
- train the Person Groups
- and identify people
