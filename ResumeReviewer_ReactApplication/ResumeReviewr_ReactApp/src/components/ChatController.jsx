import { useState, useEffect } from "react"
import UploadBar from "./UploadBar";
import ChatMessage from "./ChatMessage";

function ChatController() {
    const [messages, setMessages] = useState([])
    const [loading, setLoading] = useState(false)

    const handleUpload = async (file) => {
        if (!file) return;

        // Add user message
        setMessages((prev) => [
            ...prev,
            { role: "user", content: `Uploaded file: ${file.name}` },
        ]);
        setLoading(true);
        console.log(messages);
        try {
            const formData = new FormData();
            const uploadedFile = formData.append("file", file);
            console.log(formData);
            
            // api call to upload file

            const response = await fetch("https://localhost:7212/api/resume/uploadResume", {
                method: "POST",
                body: formData,
            });

            if (!response.ok) throw new Error("Upload failed");

            const data = await response.json(); // { ResumeId, Message }
            console.log(data);

            // Assistant feedback
            setMessages((prev) => [
                ...prev,
                { role: "assistant", content: `✅ Resume uploaded! Resume ID: ${data.resumeId}\nAnalyzing...` },
            ]);

            // Fetch analysis
            const analysisResp = await fetch(`https://localhost:7212/api/resume/${data.resumeId}/analysis`);
            const analysis = await analysisResp.json();

            const feedback = ` Experience Level: ${analysis.experienceLevel}
                               Score: ${analysis.score}
                               Strengths: ${analysis.strengths.join(", ")}
                               Improvements: ${analysis.improvements.join(", ")}  `;           

            setMessages((prev) => [
                ...prev,
                { role: "assistant", content: feedback },
            ]);

        }
        catch (err) {
            setMessages((prev) => [
                ...prev,
                { role: "assistant", content: `❌ Error: ${err.message}` },
            ]);

        }
        finally {
            setLoading(false);
        }
    }

    return (
        <>
            <div className="flex flex-col h-full">
                <div className="flex-1 p-6 overflow-y-auto space-y-2 scrollbar-thin scrollbar-thumb-gray-600 scrollbar-track-gray-500">
                    {messages.map((msg, idx) => (
                        <ChatMessage key={idx} role={msg.role} content={msg.content} />
                    ))}
                </div>

                <UploadBar onUpload={handleUpload} loading={loading} />                
            </div>
        </>
    )
}

export default ChatController