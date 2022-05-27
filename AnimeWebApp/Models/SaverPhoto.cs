namespace AnimeWebApp.Models
{
    public class SaverPhoto
    {
        public void SaveFhotoFromStream(Stream stream,string path)
        {
            using (var streamFile = new FileStream(path, FileMode.Create))
            {
                stream.CopyTo(streamFile);
            }
        }
    }
}
