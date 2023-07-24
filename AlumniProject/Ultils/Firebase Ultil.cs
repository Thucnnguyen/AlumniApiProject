using AlumniProject.ExceptionHandler;
using Firebase.Storage;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;

namespace AlumniProject.Ultils
{
    public class Firebase_Ultil
    {
        public static async Task<string> UploadImageToFirebaseStorage(IFormFile file)
        {
            if (file == null || file.Length <= 0)
            {
                throw new BadRequestException("image has problem");
            }

            // Generate a unique name for the file in Firebase Storage.
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            // Get a reference to the Firebase Storage root.
            var firebaseStorage = new FirebaseStorage("alumniproject-c8ba6.appspot.com");

            // Get a reference to the location where you want to store the file (e.g., "files" folder).
            var storageReference = firebaseStorage.Child("files").Child(fileName);

            // Upload the file to Firebase Storage.
            using (var stream = file.OpenReadStream())
            {
                await storageReference.PutAsync(stream);
            }

            // Get the public URL of the uploaded file.
            string downloadUrl = await storageReference.GetDownloadUrlAsync();
            return downloadUrl;
        }
    }
}
