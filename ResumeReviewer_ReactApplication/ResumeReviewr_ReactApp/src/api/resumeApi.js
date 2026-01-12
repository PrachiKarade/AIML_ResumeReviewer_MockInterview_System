const API_BASE = "https://localhost:7212/api";

export async function uploadResume(file) 
{
  const formData = new FormData();
  formData.append("file", file);

  const res = await fetch(`${API_BASE}/Resume/uploadResume`, 
                            {
                                method: "POST",
                                body: formData
                            });

  if (!res.ok) 
    throw new Error("Upload failed");

  return res.json();
}

export async function analyzeResume(resumeId) 
{
  const res = await fetch(`${API_BASE}/Resume/${resumeId}/analysis`);
  if (!res.ok) 
    throw new Error("Analysis failed");

  return res.json();
}

export async function startInterview(resumeId) 
{
//  "https://localhost:7212/api/Interview/start/1"
   const res = await fetch(`${API_BASE}/Interview/start/${resumeId}`, { method: "POST"});
  return res.json();
}

export async function continueInterview(resumeId, answer) 
{
  const res = await fetch(`${API_BASE}/Interview/continue/${resumeId}`, 
                            {
                                method: "POST",
                                headers: { "Content-Type": "application/json" },
                                body: JSON.stringify({ userAnswer: answer })
                            });

  return res.json();
}
