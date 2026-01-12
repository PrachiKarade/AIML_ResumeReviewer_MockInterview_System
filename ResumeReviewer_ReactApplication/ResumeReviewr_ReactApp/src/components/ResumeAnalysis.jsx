export default function ResumeAnalysis({ analysis, onStartInterview }) 
{
  return (
    <div className="p-6 bg-white shadow rounded mt-6">
        <h2 className="text-xl font-semibold mb-2">Resume Analysis</h2>

        <p><strong>Score:</strong> {analysis.score}</p>
        <p><strong>Experience:</strong> {analysis.experienceLevel}</p>

        <div className="mt-3">
            <strong>Strengths</strong>
            <ul className="list-disc ml-5">
            {analysis.strengths.map((s, i) => <li key={i}>{s}</li>)}
            </ul>
        </div>

        <div className="mt-3">
            <strong>Improvements</strong>
            <ul className="list-disc ml-5">
            {analysis.improvements.map((i, idx) => <li key={idx}>{i}</li>)}
            </ul>
        </div>

        <button onClick={onStartInterview} className="mt-4 bg-green-500 text-white px-4 py-2 rounded" >
            Start Interview
        </button>
    </div>
  );
}
