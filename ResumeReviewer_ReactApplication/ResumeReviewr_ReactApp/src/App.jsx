import { useState } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from '/vite.svg'
import './App.css'
import ChatMessage from './components/ChatMessage'
import UploadBar from './components/UploadBar'
import ChatController from './components/ChatController'
import ResumeAnalysis from './components/ResumeAnalysis'
import ResumeFlow from './Pages/ResumeFlow'
/* <ChatController/> */

function App() {
   return (
    <>
      <div>
        <ResumeFlow/>        
      </div>
    </>
  )
}
export default App
