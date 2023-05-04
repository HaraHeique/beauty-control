namespace BeautyControl.API.Features.Products._Common.Uploads
{
    public interface IImageUploadRequest
    {
        public int? Id { get; init; }
        public string? Image { get; set; }
        public string? ImageUrlUpload { get; set; }
        public IFormFile? ImageUpload { get; init; }
    }
}
