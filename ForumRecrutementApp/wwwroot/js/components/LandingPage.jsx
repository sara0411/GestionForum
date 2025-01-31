'use strict';

const e = React.createElement;

const LandingPage = () => {
    return e('div', { className: 'min-h-screen bg-gray-50' },
        // Hero Section
        e('div', { className: 'bg-white' },
            e('div', { className: 'max-w-7xl mx-auto px-4 py-16 sm:px-6 lg:px-8' },
                e('div', { className: 'text-center' },
                    e('h1', { className: 'text-4xl font-bold text-gray-900 sm:text-5xl md:text-6xl' },
                        'Forum de Recrutement'
                    ),
                    e('p', { className: 'mt-3 max-w-md mx-auto text-base text-gray-500 sm:text-lg md:mt-5 md:text-xl md:max-w-3xl' },
                        'Connectez-vous avec les meilleures entreprises et talents. Simplifiez votre processus de recrutement en temps réel.'
                    ),
                    e('div', { className: 'mt-5 max-w-md mx-auto flex justify-center gap-3' },
                        e('button', {
                            className: 'inline-flex items-center px-6 py-3 border border-transparent text-base font-medium rounded-md text-white bg-blue-600 hover:bg-blue-700',
                            onClick: () => window.location.href = '/Candidate/Candidats/Create'
                        }, 'Déposer CV'),
                        e('button', {
                            className: 'inline-flex items-center px-6 py-3 border border-transparent text-base font-medium rounded-md text-white bg-blue-600 hover:bg-blue-700',
                            onClick: () => window.location.href = '/Account/Register'
                        }, 'Register Recruteur'),
                        e('button', {
                            className: 'inline-flex items-center px-6 py-3 border border-transparent text-base font-medium rounded-md text-blue-600 bg-blue-100 hover:bg-blue-200',
                            onClick: () => window.location.href = '/Account/Login'
                        }, 'Connexion Recruteur')
                    )
                )
            )
        ),

        // Stats Section
        e('div', { className: 'bg-blue-600' },
            e('div', { className: 'max-w-7xl mx-auto py-12 px-4 sm:px-6 lg:px-8' },
                e('div', { className: 'grid grid-cols-1 gap-8 sm:grid-cols-3' },
                    e('div', { className: 'text-center' },
                        e('p', { className: 'mt-2 text-3xl font-bold text-white' }, '500+'),
                        e('p', { className: 'mt-1 text-xl text-blue-100' }, 'Candidats')
                    ),
                    e('div', { className: 'text-center' },
                        e('p', { className: 'mt-2 text-3xl font-bold text-white' }, '50+'),
                        e('p', { className: 'mt-1 text-xl text-blue-100' }, 'Entreprises')
                    ),
                    e('div', { className: 'text-center' },
                        e('p', { className: 'mt-2 text-3xl font-bold text-white' }, '100+'),
                        e('p', { className: 'mt-1 text-xl text-blue-100' }, 'Entretiens Réalisés')
                    )
                )
            )
        ),

        // Features Section
        e('div', { className: 'py-16 bg-white' },
            e('div', { className: 'max-w-7xl mx-auto px-4 sm:px-6 lg:px-8' },
                e('div', { className: 'text-center' },
                    e('h2', { className: 'text-3xl font-bold text-gray-900' }, 'Comment ça marche?')
                ),
                e('div', { className: 'mt-12 grid gap-8 grid-cols-1 md:grid-cols-3' },
                    e('div', { className: 'text-center' },
                        e('div', { className: 'flex justify-center' },
                            e('div', { className: 'flex items-center justify-center h-12 w-12 rounded-md bg-blue-500 text-white' }, '1')
                        ),
                        e('h3', { className: 'mt-4 text-xl font-medium text-gray-900' }, 'Déposez votre CV'),
                        e('p', { className: 'mt-2 text-base text-gray-500' },
                            'Remplissez le formulaire et téléchargez votre CV en quelques clics'
                        )
                    ),
                    e('div', { className: 'text-center' },
                        e('div', { className: 'flex justify-center' },
                            e('div', { className: 'flex items-center justify-center h-12 w-12 rounded-md bg-blue-500 text-white' }, '2')
                        ),
                        e('h3', { className: 'mt-4 text-xl font-medium text-gray-900' }, 'Sélectionnez vos entreprises'),
                        e('p', { className: 'mt-2 text-base text-gray-500' },
                            'Choisissez les entreprises qui vous intéressent'
                        )
                    ),
                    e('div', { className: 'text-center' },
                        e('div', { className: 'flex justify-center' },
                            e('div', { className: 'flex items-center justify-center h-12 w-12 rounded-md bg-blue-500 text-white' }, '3')
                        ),
                        e('h3', { className: 'mt-4 text-xl font-medium text-gray-900' }, 'Participez aux entretiens'),
                        e('p', { className: 'mt-2 text-base text-gray-500' },
                            'Rencontrez les recruteurs lors du forum'
                        )
                    )
                )
            )
        ),

        // Footer
        e('footer', { className: 'bg-gray-50' },
            e('div', { className: 'max-w-7xl mx-auto py-12 px-4 sm:px-6 lg:px-8' },
                e('p', { className: 'text-center text-base text-gray-500' },
                    '© 2025 ForumRecrutementApp. Tous droits réservés.'
                )
            )
        )
    );
};

const domContainer = document.querySelector('#react-landing-page');
ReactDOM.render(e(LandingPage), domContainer);