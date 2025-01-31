import { createRoot } from 'react-dom/client';
import { LandingPage } from './components/LandingPage';

const container = document.getElementById('react-landing-page');
const root = createRoot(container);
root.render(<LandingPage />);