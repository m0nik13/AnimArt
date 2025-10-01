document.addEventListener('DOMContentLoaded', function() {
    // Робимо всі картки аніме клікабельними
    const animeCards = document.querySelectorAll('.anime-card');
    
    animeCards.forEach(card => {
        card.addEventListener('click', function() {
            const animeId = this.getAttribute('data-id');
            const animeTitle = this.querySelector('.anime-title').textContent;
            
            // Тут можна додати перехід на сторінку з деталями аніме
            // Наразі просто показуємо повідомлення
            showAnimeDetails(animeId, animeTitle);
        });
    });
    
    // Додаємо обробники для навігації
    const navItems = document.querySelectorAll('.nav-item, .sidebar-item');
    
    navItems.forEach(item => {
        item.addEventListener('click', function(e) {
            e.preventDefault();
            
            // Видаляємо активний клас з усіх елементів
            navItems.forEach(navItem => navItem.classList.remove('active'));
            
            // Додаємо активний клас до поточного елемента
            this.classList.add('active');
            
            // Тут можна додати логіку завантаження відповідного контенту
            const sectionName = this.textContent;
            console.log(`Перехід до розділу: ${sectionName}`);
        });
    });
    
    // Функція для показу деталей аніме
    function showAnimeDetails(id, title) {
        // Можна замінити на перехід на окрему сторінку
        // або відкриття модального вікна
        alert(`Детальна інформація про аніме "${title}" (ID: ${id})\n\nФункціонал буде реалізовано найближчим часом!`);
        
        // Приклад переходу на сторінку деталей:
        // window.location.href = `anime-details.html?id=${id}`;
    }
    
    // Додаємо ефект завантаження карток
    setTimeout(() => {
        animeCards.forEach((card, index) => {
            card.style.opacity = '0';
            card.style.transform = 'translateY(10px)';
            
            setTimeout(() => {
                card.style.transition = 'opacity 0.3s, transform 0.3s';
                card.style.opacity = '1';
                card.style.transform = 'translateY(0)';
            }, 100 * index);
        });
    }, 300);
    
    console.log('AnimeHub завантажено успішно!');
});