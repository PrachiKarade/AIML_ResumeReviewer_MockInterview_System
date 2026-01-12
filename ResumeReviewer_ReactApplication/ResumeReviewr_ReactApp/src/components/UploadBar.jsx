import { useState } from "react";

export default function UploadBar({ onUpload, loading }) {
 
  const[file,setFile] = useState();
  
  const handleChage=(e)=>{
    setFile(e.target.files[0])
  }

  function handleClick()
  {
    if(file)
      onUpload(file);
  }

  return (
      <div className="flex items-center gap-2 p-3 border-t border-gray-600 bg-[#525366]">
        
        <input  type="file"  accept=".pdf,.doc,.docx"  className="text-sm text-gray-300"
          onChange={handleChage}   />

        <button     className="bg-green-600 px-4 py-2 rounded text-white text-sm hover:bg-green-500 disabled:opacity-50"
          disabled={loading} onClick={handleClick}   >
          {loading ? "Analyzing..." : "Upload"}
        </button>

      </div>
  );
}
