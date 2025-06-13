import React, { useState, useEffect, useRef } from 'react';
import '../styles/ImageModels.css';
import Navbar from '../components/Navbar';
import Footer from '../components/Footer';

const models = [
  {
    id: 1,
    name: "Brain Tumor Detection",
    description: "Detect brain tumors from MRI scans",
    responseType: 'image',
    endpoint: "https://clinica.runasp.net/api/Brain/predict"
  },
  {
    id: 2,
    name: "Brain Tumor Detection By Segmentation",
    description: "Analyze brain MRI scans with detailed segmentation",
    responseType: 'segmentation',
    maskEndpoint: "https://clinica.runasp.net/api/Brain/Segmentation/Mask",
    overlayEndpoint: "https://clinica.runasp.net/api/Brain/Segmentation/Overlay"
  },
  {
    id: 3,
    name: "Chest X-Ray Analysis",
    description: "Analyze chest X-rays for various conditions",
    responseType: 'text',
    endpoint: "https://clinica.runasp.net/api/Xray/classify"
  }
];

const ImageModels = () => {
  const [selectedModel, setSelectedModel] = useState(null);
  const [selectedFile, setSelectedFile] = useState(null);
  const [previewUrl, setPreviewUrl] = useState('');
  const [isAnalyzing, setIsAnalyzing] = useState(false);
  const [result, setResult] = useState(null);
  const fileInputRef = useRef(null);

  const handleModelSelect = (model) => {
    setSelectedModel(model);
    setResult(null);
  };

  const handleFileSelect = (file) => {
    if (file && file.type.startsWith('image/')) {
      setSelectedFile(file);
      const reader = new FileReader();
      reader.onload = () => setPreviewUrl(reader.result);
      reader.readAsDataURL(file);
      setResult(null);
    }
  };

  const handleDrop = (e) => {
    e.preventDefault();
    const file = e.dataTransfer.files[0];
    handleFileSelect(file);
  };

  const handleAnalysis = async () => {
    if (!selectedModel || !selectedFile) return;

    setIsAnalyzing(true);
    setResult(null);

    try {
      const formData = new FormData();
      formData.append('File', selectedFile);

      if (selectedModel.responseType === 'segmentation') {
        const [maskResponse, overlayResponse] = await Promise.all([
          fetch(selectedModel.maskEndpoint, {
            method: 'POST',
            body: formData,
            headers: {
              'Accept': '*/*',
            }
          }),
          fetch(selectedModel.overlayEndpoint, {
            method: 'POST',
            body: formData,
            headers: {
              'Accept': '*/*',
            }
          })
        ]);

        if (!maskResponse.ok || !overlayResponse.ok) {
          const maskError = await maskResponse.text();
          const overlayError = await overlayResponse.text();
          throw new Error(`API Error: ${maskResponse.status} - ${maskError || 'Failed to get mask image'} or ${overlayResponse.status} - ${overlayError || 'Failed to get overlay image'}`);
        }

        const maskBlob = await maskResponse.blob();
        const overlayBlob = await overlayResponse.blob();

        setResult({
          maskedImage: URL.createObjectURL(maskBlob),
          overlayImage: URL.createObjectURL(overlayBlob),
          text: "Segmentation analysis complete. Both masked and overlay images are shown above."
        });
      } else if (selectedModel.responseType === 'image') {
        const response = await fetch(selectedModel.endpoint, {
          method: 'POST',
          body: formData,
          headers: {
            'Accept': '*/*',
          }
        });

        if (!response.ok) {
          const errorData = await response.text();
          throw new Error(`API Error: ${response.status} - ${errorData || 'Failed to analyze'}`);
        }

        const imageBlob = await response.blob();
        setResult({
          image: URL.createObjectURL(imageBlob),
          text: "Analysis complete. The processed image is shown above."
        });
      } else {
        const response = await fetch(selectedModel.endpoint, {
          method: 'POST',
          body: formData,
          headers: {
            'Accept': '*/*',
          }
        });

        if (!response.ok) {
          const errorData = await response.text();
          throw new Error(`API Error: ${response.status} - ${errorData || 'Failed to analyze'}`);
        }

        const data = await response.json();
        const prediction = data.prediction;
        const confidence = (data.confidence * 100).toFixed(2);
        
        setResult({
          image: previewUrl,
          prediction: prediction,
          confidence: confidence,
          status: prediction === 'NORMAL' ? 'normal' : 'pneumonia',
          details: {
            title: prediction === 'NORMAL' ? 'Normal Chest X-Ray' : 'Pneumonia Detected',
            description: prediction === 'NORMAL' 
              ? 'No signs of pneumonia detected in the X-ray.'
              : 'Signs of pneumonia have been detected in the X-ray.',
            recommendation: prediction === 'NORMAL'
              ? 'Please consult with a healthcare professional for a comprehensive evaluation and to confirm the results.'
              : 'Please consult with a healthcare professional immediately for further evaluation and treatment options.'
          }
        });
      }

    } catch (error) {
      console.error('Analysis Error:', error);
      setResult({
        image: previewUrl,
        text: `Error analyzing image: ${error.message}`
      });
    } finally {
      setIsAnalyzing(false);
    }
  };

  useEffect(() => {
    const handlePaste = (e) => {
      const items = e.clipboardData.items;
      for (let i = 0; i < items.length; i++) {
        if (items[i].type.startsWith('image/')) {
          const file = items[i].getAsFile();
          if (file) {
            handleFileSelect(file);
          }
          break;
        }
      }
    };
    document.addEventListener('paste', handlePaste);
    return () => document.removeEventListener('paste', handlePaste);
  }, []);

  return (
    <>
      <Navbar />
      <div className="model-page">
        <h1>Medical Image Analysis</h1>
        <div className="model-container">
          <div className="model-selection">
            <h2>Select Model</h2>
            <div className="model-grid">
              {models.map(model => (
                <div
                  key={model.id}
                  className={`model-card ${selectedModel?.id === model.id ? 'selected' : ''}`}
                  onClick={() => handleModelSelect(model)}
                >
                  <h3>{model.name}</h3>
                  <p>{model.description}</p>
                </div>
              ))}
            </div>
          </div>

          <div className="image-upload-section">
            <div
              className="drop-zone"
              onDragOver={(e) => e.preventDefault()}
              onDrop={handleDrop}
              onClick={() => fileInputRef.current?.click()}
            >
              {!previewUrl ? (
                <div className="upload-prompt">
                  <svg className="upload-icon" viewBox="0 0 24 24">
                    <path d="M19.35 10.04C18.67 6.59 15.64 4 12 4 9.11 4 6.6 5.64 5.35 8.04 2.34 8.36 0 10.91 0 14c0 3.31 2.69 6 6 6h13c2.76 0 5-2.24 5-5 0-2.64-2.05-4.78-4.65-4.96zM14 13v4h-4v-4H7l5-5 5 5h-3z" />
                  </svg>
                  <p>Drag & drop, paste, or click to upload an image</p>
                </div>
              ) : (
                <img src={previewUrl} alt="Preview" className="preview-image" />
              )}
              <input
                type="file"
                ref={fileInputRef}
                accept="image/*"
                style={{ display: 'none' }}
                onChange={(e) => e.target.files && handleFileSelect(e.target.files[0])}
              />
            </div>

            <button
              className="analyze-button"
              disabled={!selectedModel || !selectedFile || isAnalyzing}
              onClick={handleAnalysis}
            >
              {isAnalyzing ? 'Processing...' : 'Analyze Image'}
            </button>
          </div>

          {result && (
            <div className="result-section">
              <h2>Analysis Result</h2>
              {selectedModel?.responseType === 'segmentation' ? (
                <div className="segmentation-results">
                  <div className="segmentation-image">
                    <h3>Masked Image</h3>
                    <img src={result.maskedImage} alt="Masked Result" className="result-image" />
                  </div>
                  <div className="segmentation-image">
                    <h3>Overlay Image</h3>
                    <img src={result.overlayImage} alt="Overlay Result" className="result-image" />
                  </div>
                  <p className="result-text">{result.text}</p>
                </div>
              ) : selectedModel?.responseType === 'text' && result.prediction ? (
                <div className="result-container">
                  <div className="image-container">
                    <img src={result.image} alt="X-Ray" className="result-image" />
                  </div>
                  <div className={`analysis-details ${result.status}`}>
                    <div className="prediction-header">
                      <h3>{result.details.title}</h3>
                      <div className="confidence-badge">
                        Confidence: {result.confidence}%
                      </div>
                    </div>
                    <p className="description">{result.details.description}</p>
                    <div className="recommendation">
                      <strong>Recommendation:</strong>
                      <p>{result.details.recommendation}</p>
                    </div>
                  </div>
                </div>
              ) : (
                <>
                  <img src={result.image} alt="Result" className="result-image" />
                  <p className="result-text" style={{ whiteSpace: 'pre-wrap' }}>{result.text}</p>
                </>
              )}
            </div>
          )}
        </div>
      </div>
      <Footer />
    </>
  );
};

export default ImageModels;