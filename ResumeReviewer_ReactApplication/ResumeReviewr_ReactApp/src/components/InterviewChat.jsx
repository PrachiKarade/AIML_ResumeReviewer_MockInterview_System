import { useEffect, useRef, useState } from "react";
import { startInterview, continueInterview } from "../api/resumeApi";

export default function InterviewChat({ resumeId }) 
{
  const [messages, setMessages] = useState([]);
  const [input, setInput]       = useState("");
  const bottomRef   = useRef();
  const textareaRef = useRef(null);

  useEffect(() => {
    async function init() 
    {
      const res = await startInterview(resumeId);
      setMessages([{ role: "assistant", text: res.interviewQuestion }]);
    }
    init();
  }, [resumeId]);

  useEffect(() => {
    bottomRef.current?.scrollIntoView({ behavior: "smooth" });
  }, [messages]);

  const send = async () => {
    if (!input) return;

    setMessages(m => [...m, { role: "user", text: input }]);

    const res = await continueInterview(resumeId, input);

    setMessages(m => [...m, { role: "assistant", text: res.interviewQuestion }]);

    setInput("");
  };

  

  const handleChange = (e) => {
    //onChange(e.target.value);
    setInput(e.target.value);

    // Auto resize
    textareaRef.current.style.height = "auto";
    textareaRef.current.style.height = textareaRef.current.scrollHeight + "px";
  };

  const handleKeyDown = (e) => {
    if (e.key === "Enter" && !e.shiftKey) {
      e.preventDefault();
      send();
    }
  };

  return (
            <div className="max-w-2xl mx-auto mt-6 bg-gray-100 p-4 rounded shadow">
                <h2 className="text-xl font-semibold mb-3">AI Interview</h2>

                <div className="max-h-96 overflow-y-auto space-y-3">
                    {
                        messages.map((m, i) => (
                                <div key={i} className={`flex ${m.role === "assistant" ? "justify-start" : "justify-end"}`}>
                                    <div className={`max-w-[70%] p-3 rounded-lg 
                                         ${m.role === "assistant" ? "bg-white shadow" : "bg-blue-500 text-white"}`}>
                                        {m.text}
                                    </div>
                                </div>
                        ) ) 
                    }
                    <div ref={bottomRef} />
                </div>

                <div className="flex mt-3 gap-2">
                    {
                        //<input  value={input}
                          //  onChange={e => setInput(e.target.value)}
                            //className="flex-1 p-2 border rounded"
                            //placeholder="Type your answer..."  /> 
                            
                    }

                    <textarea  ref={textareaRef}  
                               rows={1}
                               value={input}
                               onChange={handleChange}
                               onKeyDown={handleKeyDown}
                               placeholder="Type your answer..."
                               className="flex-1 resize-none rounded-xl border  px-4 py-3
                                          text-sm leading-relaxed focus:outline-none
                                          focus:ring-2 focus:ring-blue-500 max-h-48 overflow-y-auto">
                    </textarea>

                    <button onClick={send} 
                        className="self-end h-11 min-w-[64px]  bg-blue-600  hover:bg-blue-700
                                   text-white px-4 rounded-xl transition flex items-center justify-center" >
                        Send
                    </button>
                </div>
            </div>
  );
}
