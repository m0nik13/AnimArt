//wwwroot/js/site.js

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

            navItems.forEach(navItem => navItem.classList.remove('active'));

            this.classList.add('active');

            const sectionName = this.textContent;
            console.log(`Перехід до розділу: ${sectionName}`);
        });
    });

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
function initializeBurgerMenu() {
    const burgerMenu = document.querySelector('.burger-menu');
    const mobileNav = document.querySelector('.mobile-nav');
    const body = document.body;
    const header = document.querySelector('.header');

    if (!burgerMenu || !mobileNav || !header) return;

    // Функція для відкриття/закриття меню
    function toggleMenu() {
        const isExpanded = burgerMenu.getAttribute('aria-expanded') === 'true';
        const headerHeight = header.offsetHeight;

        if (!isExpanded) {
            // Відкриваємо меню
            burgerMenu.setAttribute('aria-expanded', 'true');
            mobileNav.classList.add('active');
            body.classList.add('menu-open');

            // Встановлюємо правильне позиціонування для мобільної навігації
            mobileNav.style.top = headerHeight + 'px';
            mobileNav.style.height = `calc(100vh - ${headerHeight}px)`;

            // Створюємо оверлей
            const overlay = document.createElement('div');
            overlay.className = 'mobile-nav-overlay active';
            overlay.style.top = headerHeight + 'px';
            document.body.appendChild(overlay);

            // Додаємо обробник кліку на оверлей
            overlay.addEventListener('click', closeMenu);

            // Додаємо обробник клавіші Escape
            document.addEventListener('keydown', handleEscape);
        } else {
            closeMenu();
        }
    }

    // Функція для закриття меню
    function closeMenu() {
        burgerMenu.setAttribute('aria-expanded', 'false');
        mobileNav.classList.remove('active');
        body.classList.remove('menu-open');

        // Скидаємо стилі
        mobileNav.style.top = '';
        mobileNav.style.height = '';

        // Видаляємо оверлей
        const overlay = document.querySelector('.mobile-nav-overlay');
        if (overlay) {
            overlay.remove();
        }

        // Видаляємо обробник Escape
        document.removeEventListener('keydown', handleEscape);
    }

    // Обробник клавіші Escape
    function handleEscape(event) {
        if (event.key === 'Escape') {
            closeMenu();
        }
    }

    // Додаємо обробник кліку на burger menu
    burgerMenu.addEventListener('click', toggleMenu);

    // Додаємо обробники кліку на мобільні посилання
    const mobileNavItems = document.querySelectorAll('.mobile-nav-item');
    mobileNavItems.forEach(item => {
        item.addEventListener('click', function (e) {
            // Якщо посилання веде на якорь, не закриваємо меню
            if (this.getAttribute('href') && this.getAttribute('href').startsWith('#')) {
                return;
            }

            // Закриваємо меню при кліку на посилання
            setTimeout(() => {
                closeMenu();
            }, 300);
        });
    });

    // Адаптація при зміні розміру вікна
    window.addEventListener('resize', function () {
        adjustMobileLayout();
        if (window.innerWidth > 768) {
            closeMenu();
        }
    });
}
function adjustMobileLayout() {
    const header = document.querySelector('.header');
    const content = document.querySelector('.content');
    const mobileNav = document.querySelector('.mobile-nav');

    if (!header || !content) return;

    // Перевіряємо, чи ми на мобільному пристрої
    const isMobile = window.innerWidth <= 768;

    if (isMobile) {
        // Розраховуємо висоту header
        const headerHeight = header.offsetHeight;

        // Встановлюємо правильний відступ для контенту
        content.style.paddingTop = headerHeight + 10 + 'px'; // +10px для додаткового простору

        // Корегуємо висоту мобільної навігації
        if (mobileNav) {
            mobileNav.style.top = headerHeight + 'px';
            mobileNav.style.height = `calc(100vh - ${headerHeight}px)`;
        }
    } else {
        // Скидаємо стилі для десктопу
        content.style.paddingTop = '';
        if (mobileNav) {
            mobileNav.style.top = '';
            mobileNav.style.height = '';
        }
    }
}
// Оновлена функція ініціалізації головної сторінки
function initializeHomePage() {
    const animeCards = document.querySelectorAll('.anime-card');

    animeCards.forEach(card => {
        // Ефект при наведенні миші
        card.addEventListener('mouseenter', function () {
            this.style.transform = 'translateY(-5px) scale(1.009)';
            this.style.boxShadow = '0 8px 20px rgba(187, 134, 252, 0.25)';
            this.style.zIndex = '10';

            // Додатковий ефект для постеру
            const poster = this.querySelector('.anime-poster img');
            if (poster) {
                poster.style.transform = 'scale(1.04)';
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

    // Анімація завантаження карток
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
}

// Оновлена функція ініціалізації всієї сторінки
document.addEventListener('DOMContentLoaded', function () {
    // Ініціалізація burger menu
    initializeBurgerMenu();

    // Перевіряємо, чи ми на головній сторінці
    const animeList = document.querySelector('.anime-list');
    if (animeList) {
        initializeHomePage();
    }

    // Додаємо обробники для навігації
    const navItems = document.querySelectorAll('.nav-item, .sidebar-item, .mobile-nav-item');

    navItems.forEach(item => {
        item.addEventListener('click', function (e) {
            // Оновлюємо активний стан (якщо потрібно)
            navItems.forEach(navItem => navItem.classList.remove('active'));
            this.classList.add('active');
        });
    });

    console.log('AnimArt завантажено успішно! Burger menu активовано.');
});
window.closeModal = function (button) {
    const modal = button.closest('.modal-container');
    if (modal) {
        modal.style.opacity = '0';
        setTimeout(() => modal.remove(), 300);
    }
};