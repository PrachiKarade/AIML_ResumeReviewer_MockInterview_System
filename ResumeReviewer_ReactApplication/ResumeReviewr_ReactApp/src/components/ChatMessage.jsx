function ChatMessage({ role, content }) {
    const isUser = role === "user";
    return (
        <>
            <div className={`max-w-[70%] px-4 py-3 rounded-lg text-sm whitespace-pre-wrap
                    ${isUser
                    ? "bg-blue-600 text-white rounded-br-none"
                    : "bg-[#5b5c6b] text-gray-100 rounded-bl-none"}`}>
                {content}
            </div>
        </>
    )
}

export default ChatMessage