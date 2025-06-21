<<<<<<< HEAD
import React, { useState, useEffect, useRef } from 'react';
import '../styles/ChatBot.css';
import axios from 'axios';

axios.defaults.baseURL = "https://clinica.runasp.net/api";

const CHAT_MODEL_ENDPOINT = '/Chatbot/send';
const API_TIMEOUT = 30000;

const sendMessageToAPI = async (userMessage) => {
    const controller = new AbortController();
    const timeoutId = setTimeout(() => controller.abort(), API_TIMEOUT);

    try {
        const response = await axios.post(CHAT_MODEL_ENDPOINT, {
            question: userMessage
        }, {
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${localStorage.getItem('token')}`
            },
            signal: controller.signal
        });

        if (response.data && typeof response.data === 'object' && 'reply' in response.data) {
            return response.data.reply;
        }
        
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
    const [isMinimized, setIsMinimized] = useState(true);
    const [messages, setMessages] = useState([]);
    const [inputValue, setInputValue] = useState('');
    const [isWaitingForResponse, setIsWaitingForResponse] = useState(false);
    const messagesContainerRef = useRef(null);
    const chatContainerRef = useRef(null);

    useEffect(() => {   
        const handleClickOutside = (e) => {
            if (!isMinimized && 
                chatContainerRef.current && 
                !chatContainerRef.current.contains(e.target)) {
                toggleChat();
            }
        };

        document.addEventListener('click', handleClickOutside);
        return () => document.removeEventListener('click', handleClickOutside);
    }, [isMinimized]);

    useEffect(() => {
        // Auto-scroll to bottom when new messages arrive
        if (messagesContainerRef.current) {
            messagesContainerRef.current.scrollTop = messagesContainerRef.current.scrollHeight;
        }
    }, [messages]);

    const toggleChat = () => {
        if (isMinimized && messages.length === 0) {
            setMessages([{
                text: "Welcome to Clinica! How can I assist you today?",
                type: 'bot-message'
            }]);
        }
        setIsMinimized(!isMinimized);
    };

    const handleSendMessage = async () => {
        if (isWaitingForResponse || !inputValue.trim()) return;
        
        const newMessage = { text: inputValue.trim(), type: 'user-message' };
        setMessages(prev => [...prev, newMessage]);
        setInputValue('');
        setIsWaitingForResponse(true);

        try {
            const response = await sendMessageToAPI(newMessage.text);
            setMessages(prev => [...prev, { 
                text: response,
                type: 'bot-message' 
            }]);
        } catch (error) {
            // Handle errors appropriately
            setMessages(prev => [...prev, { 
                text: "حدث خطأ في الاتصال. يرجى المحاولة مرة أخرى",
                type: 'bot-message error' 
            }]);
        } finally {
            setIsWaitingForResponse(false);
        }
    };

    const handleKeyPress = (e) => {
        if (e.key === 'Enter' && !e.shiftKey) {
            e.preventDefault();
            handleSendMessage();
        }
    };

    return (
        <div ref={chatContainerRef} className={`chat-container ${isMinimized ? 'minimized' : ''}`}>
            <div className={`chat-content ${isMinimized ? 'hidden' : ''}`}>
                <div className="chat-header">
                    <span>Medical Assistant</span>
                    <button className="minimize-button" onClick={toggleChat}>
                        <svg width="16" height="2" viewBox="0 0 16 2">
                            <path d="M0 0h16v2H0z" fill="currentColor"/>
                        </svg>
                    </button>
                </div>
                
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

            <div className={`chat-icon ${isMinimized ? 'visible' : ''}`} onClick={toggleChat}>
                <svg viewBox="0 0 24 24">
                    <path d="M20 2H4c-1.1 0-2 .9-2 2v18l4-4h14c1.1 0 2-.9 2-2V4c0-1.1-.9-2-2-2zm0 14H6l-2 2V4h16v12z"/>
                    <path d="M7 9h10M7 12h7" stroke="currentColor" strokeWidth="2" strokeLinecap="round"/>
                </svg>
            </div>
        </div>
    );
};

export default ChatBot;


=======
import React, { useState, useEffect, useRef } from 'react';
import '../styles/ChatBot.css';
import axios from 'axios';

axios.defaults.baseURL = "https://clinica.runasp.net/api";

const CHAT_MODEL_ENDPOINT = '/Chatbot/send';
const API_TIMEOUT = 30000;

const sendMessageToAPI = async (userMessage) => {
    const controller = new AbortController();
    const timeoutId = setTimeout(() => controller.abort(), API_TIMEOUT);

    try {
        const response = await axios.post(CHAT_MODEL_ENDPOINT, {
            question: userMessage
        }, {
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${localStorage.getItem('token')}`
            },
            signal: controller.signal
        });

        if (response.data && typeof response.data === 'object' && 'reply' in response.data) {
            return response.data.reply;
        }
        
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
    const [isMinimized, setIsMinimized] = useState(true);
    const [messages, setMessages] = useState([]);
    const [inputValue, setInputValue] = useState('');
    const [isWaitingForResponse, setIsWaitingForResponse] = useState(false);
    const messagesContainerRef = useRef(null);
    const chatContainerRef = useRef(null);

    useEffect(() => {   
        const handleClickOutside = (e) => {
            if (!isMinimized && 
                chatContainerRef.current && 
                !chatContainerRef.current.contains(e.target)) {
                toggleChat();
            }
        };

        document.addEventListener('click', handleClickOutside);
        return () => document.removeEventListener('click', handleClickOutside);
    }, [isMinimized]);

    useEffect(() => {
        // Auto-scroll to bottom when new messages arrive
        if (messagesContainerRef.current) {
            messagesContainerRef.current.scrollTop = messagesContainerRef.current.scrollHeight;
        }
    }, [messages]);

    const toggleChat = () => {
        if (isMinimized && messages.length === 0) {
            setMessages([{
                text: "Welcome to Clinica! How can I assist you today?",
                type: 'bot-message'
            }]);
        }
        setIsMinimized(!isMinimized);
    };

    const handleSendMessage = async () => {
        if (isWaitingForResponse || !inputValue.trim()) return;
        
        const newMessage = { text: inputValue.trim(), type: 'user-message' };
        setMessages(prev => [...prev, newMessage]);
        setInputValue('');
        setIsWaitingForResponse(true);

        try {
            const response = await sendMessageToAPI(newMessage.text);
            setMessages(prev => [...prev, { 
                text: response,
                type: 'bot-message' 
            }]);
        } catch (error) {
            // Handle errors appropriately
            setMessages(prev => [...prev, { 
                text: "حدث خطأ في الاتصال. يرجى المحاولة مرة أخرى",
                type: 'bot-message error' 
            }]);
        } finally {
            setIsWaitingForResponse(false);
        }
    };

    const handleKeyPress = (e) => {
        if (e.key === 'Enter' && !e.shiftKey) {
            e.preventDefault();
            handleSendMessage();
        }
    };

    return (
        <div ref={chatContainerRef} className={`chat-container ${isMinimized ? 'minimized' : ''}`}>
            <div className={`chat-content ${isMinimized ? 'hidden' : ''}`}>
                <div className="chat-header">
                    <span>Medical Assistant</span>
                    <button className="minimize-button" onClick={toggleChat}>
                        <svg width="16" height="2" viewBox="0 0 16 2">
                            <path d="M0 0h16v2H0z" fill="currentColor"/>
                        </svg>
                    </button>
                </div>
                
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

            <div className={`chat-icon ${isMinimized ? 'visible' : ''}`} onClick={toggleChat}>
                <svg viewBox="0 0 24 24">
                    <path d="M20 2H4c-1.1 0-2 .9-2 2v18l4-4h14c1.1 0 2-.9 2-2V4c0-1.1-.9-2-2-2zm0 14H6l-2 2V4h16v12z"/>
                    <path d="M7 9h10M7 12h7" stroke="currentColor" strokeWidth="2" strokeLinecap="round"/>
                </svg>
            </div>
        </div>
    );
};

export default ChatBot;


>>>>>>> 8353e6b8191821f7a21f7a21f1f2e1a60dab3cc6
