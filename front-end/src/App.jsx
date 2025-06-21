<<<<<<< HEAD
import React, { useState, useEffect, useRef } from 'react';
import '../styles/ChatBot.css';
import axios from 'axios';

// axios.defaults.baseURL = process.env.REACT_APP_SERVER_DOMAIN;
axios.defaults.baseURL = "https://clinica.runasp.net/api";

// تعديل المتغير الثابت للمسار
const CHAT_MODEL_ENDPOINT = '/Chatbot/send';
const API_TIMEOUT = 30000;

const sendMessageToAPI = async (userMessage) => {
    const controller = new AbortController();
    const timeoutId = setTimeout(() => controller.abort(), API_TIMEOUT);

    try {
        const response = await axios.post(CHAT_MODEL_ENDPOINT, {
            question: userMessage  // تغيير message إلى question حسب هيكل API
        }, {
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${localStorage.getItem('token')}`
            },
            signal: controller.signal
        });

        // Extract the reply from the response
        if (response.data && typeof response.data === 'object' && 'reply' in response.data) {
            return response.data.reply;
        }
        
        // Fallback if response structure is different
        return response.data;
    } catch (error) {
        if (error.name === 'AbortError') {
            throw new Error('Request timeout');
        }
        console.error('Error sending message:', error);
        throw error;
    } finally {
        clearTimeout(timeoutId);
    }
};

const ChatBot = () => {
    const [messages, setMessages] = useState([]);
    const [inputValue, setInputValue] = useState('');
    const [isWaitingForResponse, setIsWaitingForResponse] = useState(false);
    const [selectedFile, setSelectedFile] = useState(null);
    const messagesContainerRef = useRef(null);
    const fileInputRef = useRef(null);

    useEffect(() => {
        // Show welcome message on component mount
        if (messages.length === 0) {
            setMessages([{
                text: "Welcome to Clinica! How can I assist you today?",
                type: 'bot-message'
            }]);
        }
    }, []);

    useEffect(() => {
        // Auto-scroll to bottom when new messages arrive
        if (messagesContainerRef.current) {
            messagesContainerRef.current.scrollTop = messagesContainerRef.current.scrollHeight;
        }
    }, [messages]);

    const handleSendMessage = async () => {
        if (isWaitingForResponse || !inputValue.trim()) return;
        if (inputValue.length > 500) {
            setMessages(prev => [...prev, { 
                text: "Message is too long. Please limit to 500 characters.", 
                type: 'bot-message' 
            }]);
            return;
        }

        const newMessage = { text: inputValue.trim(), type: 'user-message' };
        setMessages(prev => [...prev, newMessage]);
        setInputValue('');
        setIsWaitingForResponse(true);

        const retryCount = 3;
        for (let i = 0; i < retryCount; i++) {
            try {
                const response = await sendMessageToAPI(newMessage.text);
                // Make sure response is a string before adding to messages
                const botResponse = typeof response === 'object' ? 
                    (response.reply || 'Sorry, I received an invalid response') : 
                    response;
                    
                setMessages(prev => [...prev, { 
                    text: botResponse,
                    type: 'bot-message' 
                }]);
                break;
            } catch (error) {
                console.error('Error details:', error);
                if (i === retryCount - 1) {
                    let errorMessage = "لا يمكن الاتصال بالخادم. يرجى المحاولة مرة أخرى لاحقاً.";
                    
                    if (error.response) {
                        // Server responded with error
                        switch (error.response.status) {
                            case 401:
                                errorMessage = "يرجى تسجيل الدخول للمتابعة";
                                break;
                            case 403:
                                errorMessage = "غير مصرح لك باستخدام هذه الخدمة";
                                break;
                            case 429:
                                errorMessage = "عدد كبير من الطلبات. يرجى الانتظار قليلاً";
                                break;
                            default:
                                errorMessage = "حدث خطأ في النظام. يرجى المحاولة لاحقاً";
                        }
                    } else if (error.name === 'AbortError') {
                        errorMessage = "انتهت مهلة الاتصال. يرجى المحاولة مرة أخرى";
                    }

                    setMessages(prev => [...prev, { 
                        text: errorMessage,
                        type: 'bot-message error' 
                    }]);
                }
                continue;
            }
        }
        setIsWaitingForResponse(false);
    };

    const handleClearChat = () => {
        setMessages([{
            text: "Chat cleared. How can I assist you today?",
            type: 'bot-message'
        }]);
    };

    const handleFileUpload = (event) => {
        const file = event.target.files[0];
        if (file) {
            if (file.size > 5 * 1024 * 1024) { // 5MB limit
                setMessages(prev => [...prev, {
                    text: "File size too large. Please upload files smaller than 5MB.",
                    type: 'bot-message error'
                }]);
                return;
            }
            setSelectedFile(file);
            setMessages(prev => [...prev, {
                text: `File selected: ${file.name}`,
                type: 'user-message'
            }]);
        }
    };

    const handleKeyPress = (e) => {
        if (e.key === 'Enter' && !e.shiftKey) {
            e.preventDefault();
            handleSendMessage();
        }
    };

    return (
        <div className="chat-page">
            <div className="chat-container full-page">
                <div className="chat-header">
                    <span>Medical Assistant</span>
                    <div className="header-buttons">
                        <button onClick={() => fileInputRef.current.click()} className="upload-button">
                            <svg viewBox="0 0 24 24" width="24" height="24">
                                <path d="M19.35 10.04C18.67 6.59 15.64 4 12 4 9.11 4 6.6 5.64 5.35 8.04 2.34 8.36 0 10.91 0 14c0 3.31 2.69 6 6 6h13c2.76 0 5-2.24 5-5 0-2.64-2.05-4.78-4.65-4.96zM14 13v4h-4v-4H7l5-5 5 5h-3z"/>
                            </svg>
                        </button>
                        <button onClick={handleClearChat} className="clear-button">
                            <svg viewBox="0 0 24 24" width="24" height="24">
                                <path d="M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z"/>
                            </svg>
                        </button>
                    </div>
                </div>
                
                <input
                    type="file"
                    ref={fileInputRef}
                    onChange={handleFileUpload}
                    style={{ display: 'none' }}
                    accept="image/*"
                />

                <div className="chat-messages" ref={messagesContainerRef}>
                    {messages.map((message, index) => (
                        <React.Fragment key={index}>
                            <div className={`message ${message.type}`}>
                                {message.text}
                            </div>
                            <div className="clear" />
                        </React.Fragment>
                    ))}
                    {isWaitingForResponse && (
                        <div className="typing-indicator">
                            <span></span>
                            <span></span>
                            <span></span>
                        </div>
                    )}
                </div>

                <div className="chat-input">
                    <div className="input-row">
                        <input 
                            type="text"
                            value={inputValue}
                            onChange={(e) => setInputValue(e.target.value)}
                            onKeyPress={handleKeyPress}
                            placeholder="Type your message here..."
                            disabled={isWaitingForResponse}
                            className={`message-input ${isWaitingForResponse ? 'disabled' : ''}`}
                        />
                        <button 
                            onClick={handleSendMessage}
                            disabled={isWaitingForResponse}
                            className={`send-button ${isWaitingForResponse ? 'disabled' : ''}`}
                        >
                            <svg width="20" height="20" viewBox="0 0 24 24" fill="currentColor">
                                <path d="M2.01 21L23 12 2.01 3 2 10l15 2-15 2z"/>
                            </svg>
                        </button>
                    </div>
                </div>
            </div>
        </div>
    );
};

=======
import React, { useState, useEffect, useRef } from 'react';
import '../styles/ChatBot.css';
import axios from 'axios';

// axios.defaults.baseURL = process.env.REACT_APP_SERVER_DOMAIN;
axios.defaults.baseURL = "https://clinica.runasp.net/api";

// تعديل المتغير الثابت للمسار
const CHAT_MODEL_ENDPOINT = '/Chatbot/send';
const API_TIMEOUT = 30000;

const sendMessageToAPI = async (userMessage) => {
    const controller = new AbortController();
    const timeoutId = setTimeout(() => controller.abort(), API_TIMEOUT);

    try {
        const response = await axios.post(CHAT_MODEL_ENDPOINT, {
            question: userMessage  // تغيير message إلى question حسب هيكل API
        }, {
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${localStorage.getItem('token')}`
            },
            signal: controller.signal
        });

        // Extract the reply from the response
        if (response.data && typeof response.data === 'object' && 'reply' in response.data) {
            return response.data.reply;
        }
        
        // Fallback if response structure is different
        return response.data;
    } catch (error) {
        if (error.name === 'AbortError') {
            throw new Error('Request timeout');
        }
        console.error('Error sending message:', error);
        throw error;
    } finally {
        clearTimeout(timeoutId);
    }
};

const ChatBot = () => {
    const [messages, setMessages] = useState([]);
    const [inputValue, setInputValue] = useState('');
    const [isWaitingForResponse, setIsWaitingForResponse] = useState(false);
    const [selectedFile, setSelectedFile] = useState(null);
    const messagesContainerRef = useRef(null);
    const fileInputRef = useRef(null);

    useEffect(() => {
        // Show welcome message on component mount
        if (messages.length === 0) {
            setMessages([{
                text: "Welcome to Clinica! How can I assist you today?",
                type: 'bot-message'
            }]);
        }
    }, []);

    useEffect(() => {
        // Auto-scroll to bottom when new messages arrive
        if (messagesContainerRef.current) {
            messagesContainerRef.current.scrollTop = messagesContainerRef.current.scrollHeight;
        }
    }, [messages]);

    const handleSendMessage = async () => {
        if (isWaitingForResponse || !inputValue.trim()) return;
        if (inputValue.length > 500) {
            setMessages(prev => [...prev, { 
                text: "Message is too long. Please limit to 500 characters.", 
                type: 'bot-message' 
            }]);
            return;
        }

        const newMessage = { text: inputValue.trim(), type: 'user-message' };
        setMessages(prev => [...prev, newMessage]);
        setInputValue('');
        setIsWaitingForResponse(true);

        const retryCount = 3;
        for (let i = 0; i < retryCount; i++) {
            try {
                const response = await sendMessageToAPI(newMessage.text);
                // Make sure response is a string before adding to messages
                const botResponse = typeof response === 'object' ? 
                    (response.reply || 'Sorry, I received an invalid response') : 
                    response;
                    
                setMessages(prev => [...prev, { 
                    text: botResponse,
                    type: 'bot-message' 
                }]);
                break;
            } catch (error) {
                console.error('Error details:', error);
                if (i === retryCount - 1) {
                    let errorMessage = "لا يمكن الاتصال بالخادم. يرجى المحاولة مرة أخرى لاحقاً.";
                    
                    if (error.response) {
                        // Server responded with error
                        switch (error.response.status) {
                            case 401:
                                errorMessage = "يرجى تسجيل الدخول للمتابعة";
                                break;
                            case 403:
                                errorMessage = "غير مصرح لك باستخدام هذه الخدمة";
                                break;
                            case 429:
                                errorMessage = "عدد كبير من الطلبات. يرجى الانتظار قليلاً";
                                break;
                            default:
                                errorMessage = "حدث خطأ في النظام. يرجى المحاولة لاحقاً";
                        }
                    } else if (error.name === 'AbortError') {
                        errorMessage = "انتهت مهلة الاتصال. يرجى المحاولة مرة أخرى";
                    }

                    setMessages(prev => [...prev, { 
                        text: errorMessage,
                        type: 'bot-message error' 
                    }]);
                }
                continue;
            }
        }
        setIsWaitingForResponse(false);
    };

    const handleClearChat = () => {
        setMessages([{
            text: "Chat cleared. How can I assist you today?",
            type: 'bot-message'
        }]);
    };

    const handleFileUpload = (event) => {
        const file = event.target.files[0];
        if (file) {
            if (file.size > 5 * 1024 * 1024) { // 5MB limit
                setMessages(prev => [...prev, {
                    text: "File size too large. Please upload files smaller than 5MB.",
                    type: 'bot-message error'
                }]);
                return;
            }
            setSelectedFile(file);
            setMessages(prev => [...prev, {
                text: `File selected: ${file.name}`,
                type: 'user-message'
            }]);
        }
    };

    const handleKeyPress = (e) => {
        if (e.key === 'Enter' && !e.shiftKey) {
            e.preventDefault();
            handleSendMessage();
        }
    };

    return (
        <div className="chat-page">
            <div className="chat-container full-page">
                <div className="chat-header">
                    <span>Medical Assistant</span>
                    <div className="header-buttons">
                        <button onClick={() => fileInputRef.current.click()} className="upload-button">
                            <svg viewBox="0 0 24 24" width="24" height="24">
                                <path d="M19.35 10.04C18.67 6.59 15.64 4 12 4 9.11 4 6.6 5.64 5.35 8.04 2.34 8.36 0 10.91 0 14c0 3.31 2.69 6 6 6h13c2.76 0 5-2.24 5-5 0-2.64-2.05-4.78-4.65-4.96zM14 13v4h-4v-4H7l5-5 5 5h-3z"/>
                            </svg>
                        </button>
                        <button onClick={handleClearChat} className="clear-button">
                            <svg viewBox="0 0 24 24" width="24" height="24">
                                <path d="M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z"/>
                            </svg>
                        </button>
                    </div>
                </div>
                
                <input
                    type="file"
                    ref={fileInputRef}
                    onChange={handleFileUpload}
                    style={{ display: 'none' }}
                    accept="image/*"
                />

                <div className="chat-messages" ref={messagesContainerRef}>
                    {messages.map((message, index) => (
                        <React.Fragment key={index}>
                            <div className={`message ${message.type}`}>
                                {message.text}
                            </div>
                            <div className="clear" />
                        </React.Fragment>
                    ))}
                    {isWaitingForResponse && (
                        <div className="typing-indicator">
                            <span></span>
                            <span></span>
                            <span></span>
                        </div>
                    )}
                </div>

                <div className="chat-input">
                    <div className="input-row">
                        <input 
                            type="text"
                            value={inputValue}
                            onChange={(e) => setInputValue(e.target.value)}
                            onKeyPress={handleKeyPress}
                            placeholder="Type your message here..."
                            disabled={isWaitingForResponse}
                            className={`message-input ${isWaitingForResponse ? 'disabled' : ''}`}
                        />
                        <button 
                            onClick={handleSendMessage}
                            disabled={isWaitingForResponse}
                            className={`send-button ${isWaitingForResponse ? 'disabled' : ''}`}
                        >
                            <svg width="20" height="20" viewBox="0 0 24 24" fill="currentColor">
                                <path d="M2.01 21L23 12 2.01 3 2 10l15 2-15 2z"/>
                            </svg>
                        </button>
                    </div>
                </div>
            </div>
        </div>
    );
};

>>>>>>> 8353e6b8191821f7a21f7a21f1f2e1a60dab3cc6
export default ChatBot;