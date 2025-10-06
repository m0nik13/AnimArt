document.addEventListener('DOMContentLoaded', function () {
    // Робимо всі картки аніме клікабельними
    const animeCards = document.querySelectorAll('.anime-card');

    animeCards.forEach(card => {
        // Ефект при наведенні миші
        card.addEventListener('mouseenter', function () {
            this.style.transform = 'translateY(-5px) scale(1.009)'; // Зменшено підйом
            this.style.boxShadow = '0 8px 20px rgba(187, 134, 252, 0.25)'; // Зменшено тінь
            this.style.zIndex = '10';

            // Додатковий ефект для постеру
            const poster = this.querySelector('.anime-poster img');
            if (poster) {
                poster.style.transform = 'scale(1.04)'; // Зменшено збільшення
            }

            // Ефект для заголовка
            const title = this.querySelector('.anime-title');
            if (title) {
                title.style.color = '#bb86fc';
            }
        });

        // Ефект при відведенні миші
        card.addEventListener('mouseleave', function () {
            this.style.transform = 'translateY(0) scale(1)';
            this.style.boxShadow = '0 4px 15px rgba(0, 0, 0, 0.2)';
            this.style.zIndex = '1';

            // Повертаємо постер у початковий стан
            const poster = this.querySelector('.anime-poster img');
            if (poster) {
                poster.style.transform = 'scale(1)';
            }

            // Повертаємо заголовок у початковий стан
            const title = this.querySelector('.anime-title');
            if (title) {
                title.style.color = '#e0e0e0';
            }
        });

        // Обробник кліку
        card.addEventListener('click', function () {
            const animeId = this.getAttribute('data-id');
            const animeTitle = this.querySelector('.anime-title').textContent;
            showAnimeDetails(animeId, animeTitle);
        });
    });

    // Додаємо обробники для навігації
    const navItems = document.querySelectorAll('.nav-item, .sidebar-item');

    navItems.forEach(item => {
        item.addEventListener('click', function (e) {
            e.preventDefault();

            navItems.forEach(navItem => navItem.classList.remove('active'));

            this.classList.add('active');

            const sectionName = this.textContent;
            console.log(`Перехід до розділу: ${sectionName}`);
        });
    });

    // Функція для показу деталей аніме
    function showAnimeDetails(id, title) {
        // Створюємо модальне вікно
        const modal = document.createElement('div');
        modal.style.cssText = `
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background-color: rgba(0, 0, 0, 0.8);
            display: flex;
            justify-content: center;
            align-items: center;
            z-index: 1000;
            opacity: 0;
            transition: opacity 0.1s ease;
        `;

        modal.innerHTML = `
            <div style="
                background: #252525;
                padding: 30px;
                border-radius: 12px;
                max-width: 500px;
                width: 90%;
                border-left: 4px solid #bb86fc;
                transform: scale(0.9);
                transition: transform 0.3s ease;
            ">
                <h3 style="color: #bb86fc; margin-bottom: 15px; font-size: 1.4rem;">${title}</h3>
                <p style="color: #a0a0a0; margin-bottom: 20px; line-height: 1.5;">
                    Детальна інформація про аніме "${title}" (ID: ${id}) буде доступна найближчим часом!
                </p>
                <button onclick="this.closest('div[style]').parentElement.remove()" 
                        style="
                            background: #bb86fc;
                            color: white;
                            border: none;
                            padding: 10px 20px;
                            border-radius: 6px;
                            cursor: pointer;
                            font-weight: 500;
                            transition: background 0.3s ease;
                        "
                        onmouseenter="this.style.background='#9b6bd4'" 
                        onmouseleave="this.style.background='#bb86fc'">
                    Закрити
                </button>
            </div>
        `;

        document.body.appendChild(modal);

        // Анімація появи
        setTimeout(() => {
            modal.style.opacity = '1';
            modal.querySelector('div').style.transform = 'scale(1)';
        }, 10);

        // Закриття при кліку на фон
        modal.addEventListener('click', function (e) {
            if (e.target === modal) {
                modal.remove();
            }
        });

        // Закриття при натисканні Escape
        const closeModal = (e) => {
            if (e.key === 'Escape') {
                modal.remove();
                document.removeEventListener('keydown', closeModal);
            }
        };
        document.addEventListener('keydown', closeModal);

        // Функція для закриття модального вікна з анімацією
        function closeModalWithAnimation(modal) {
            modal.style.opacity = '0';
            modal.querySelector('div').style.transform = 'scale(0.95)';
            setTimeout(() => {
                if (modal.parentElement) {
                    modal.remove();
                }
            }, 200);
        }
    }

    // Завантаження карток
    setTimeout(() => {
        animeCards.forEach((card, index) => {
            card.style.opacity = '0';
            card.style.transform = 'translateY(20px) scale(0.95)';

            setTimeout(() => {
                card.style.transition = 'opacity 0.5s ease, transform 0.5s ease, box-shadow 0.3s ease';
                card.style.opacity = '1';
                card.style.transform = 'translateY(0) scale(1)';
            }, 100 * index);
        });
    }, 200);

    console.log('AnimArt завантажено успішно!');
});

window.closeModal = function (button) {
    const modal = button.closest('.modal-container');
    if (modal) {
        modal.style.opacity = '0';
        setTimeout(() => modal.remove(), 300);
    }
};