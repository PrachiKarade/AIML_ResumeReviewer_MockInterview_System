export default function ResumeUpload({ onUploaded }) 
{
  const handleFileChange = async (e) => {
    const file = e.target.files[0];

    if (!file) return;
    onUploaded(file);
  };

  return (
            <div className="p-6 bg-white shadow rounded text-center">
                <h2 className="text-xl font-semibold mb-4">Upload Your Resume</h2>

                <input   type="file" accept=".pdf,.docx" onChange={handleFileChange} className="block mx-auto" />
            </div>
         );
}
