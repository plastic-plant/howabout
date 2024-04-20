import React from 'react'
import ReactDOM from 'react-dom/client'
//import ReactDOM from 'react-dom';
import App from './App.tsx'
import './index.css'

// React 18:
ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <App />
  </React.StrictMode>,
)

// React 17:
//ReactDOM.render(
//    <React.StrictMode>
//        <App />
//    </React.StrictMode>,
//    document.getElementById('root')
//);