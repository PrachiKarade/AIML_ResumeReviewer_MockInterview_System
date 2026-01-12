import { useState } from "react";
import ResumeUpload from "../components/ResumeUpload";
import ResumeAnalysis from "../components/ResumeAnalysis";
import InterviewChat from "../components/InterviewChat";
import { uploadResume, analyzeResume } from "../api/resumeApi";

export default function ResumeFlow() 
{
  const [resumeId, setResumeId] = useState(null);
  const [analysis, setAnalysis] = useState(null);
  const [startInterview, setStartInterview] = useState(false);

  const handleUpload = async (file) => {
    const uploadRes = await uploadResume(file);

    setResumeId(uploadRes.resumeId);

    const analysisRes = await analyzeResume(uploadRes.resumeId);

    setAnalysis(analysisRes);
  };

  return (
            <div className="min-h-screen bg-gray-50 p-6">
            {
                !analysis && <ResumeUpload onUploaded={handleUpload} />
            }

            {
                analysis && !startInterview && 
                (
                   <ResumeAnalysis  analysis={analysis} onStartInterview={() => setStartInterview(true)}  />
                )
            }

            {
                startInterview && <InterviewChat resumeId={resumeId} />
            }
            
            </div>
  );
}
