// ============================================
// Takvim Tabanlı Rezervasyon Sistemi
// ============================================

let currentDate = new Date();
let availableSlots = [];
let selectedSlot = null;

const monthNames = [
    'Ocak', 'Şubat', 'Mart', 'Nisan', 'Mayıs', 'Haziran',
    'Temmuz', 'Ağustos', 'Eylül', 'Ekim', 'Kasım', 'Aralık'
];

const dayNames = ['Paz', 'Pzt', 'Sal', 'Çar', 'Per', 'Cum', 'Cmt'];

// Sayfa yüklendiğinde
document.addEventListener('DOMContentLoaded', function() {
    console.log('Takvim sistemi yükleniyor...');
    
    // Önce slotları yükle, sonra takvimi render et
    loadAvailableSlots();
    
    // Ay navigasyonu
    const prevBtn = document.getElementById('prevMonth');
    const nextBtn = document.getElementById('nextMonth');
    
    if (prevBtn) {
        prevBtn.addEventListener('click', () => {
            currentDate.setMonth(currentDate.getMonth() - 1);
            renderCalendar();
        });
    }
    
    if (nextBtn) {
        nextBtn.addEventListener('click', () => {
            currentDate.setMonth(currentDate.getMonth() + 1);
            renderCalendar();
        });
    }
});

// Müsait slotları API'den yükle
async function loadAvailableSlots() {
    try {
        console.log('API\'den slotlar yükleniyor...');
        const response = await fetch('/api/available-slots');

        
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        
        const data = await response.json();
        availableSlots = data.slots;
        console.log('Yüklenen slot sayısı:', availableSlots.length);
        console.log('Slotlar:', availableSlots);
        
        // Slotlar yüklendikten sonra takvimi render et
        renderCalendar();
    } catch (error) {
        console.error('Slotlar yüklenemedi:', error);
        alert('Müsait zamanlar yüklenirken bir hata oluştu. Lütfen sayfayı yenileyin.');
    }
}

// Takvimi render et
function renderCalendar() {
    console.log('Takvim render ediliyor...');
    const calendar = document.getElementById('calendar');
    
    if (!calendar) {
        console.error('Takvim elementi bulunamadı!');
        return;
    }
    
    calendar.innerHTML = '';
    
    // Ay ve yıl başlığını güncelle
    const monthHeader = document.getElementById('currentMonth');
    if (monthHeader) {
        monthHeader.textContent = `${monthNames[currentDate.getMonth()]} ${currentDate.getFullYear()}`;
    }
    
    // Gün başlıklarını ekle
    dayNames.forEach(day => {
        const dayHeader = document.createElement('div');
        dayHeader.className = 'calendar-day-header';
        dayHeader.textContent = day;
        calendar.appendChild(dayHeader);
    });
    
    // Ayın ilk günü ve son günü
    const firstDay = new Date(currentDate.getFullYear(), currentDate.getMonth(), 1);
    const lastDay = new Date(currentDate.getFullYear(), currentDate.getMonth() + 1, 0);
    
    // İlk güne kadar boş hücreler ekle
    const firstDayOfWeek = firstDay.getDay();
    for (let i = 0; i < firstDayOfWeek; i++) {
        const emptyDay = document.createElement('div');
        emptyDay.className = 'calendar-day empty';
        calendar.appendChild(emptyDay);
    }
    
    // Günleri ekle
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    
    let availableDaysCount = 0;
    
    for (let day = 1; day <= lastDay.getDate(); day++) {
        const dayElement = document.createElement('div');
        dayElement.className = 'calendar-day';
        
        const dayNumber = document.createElement('div');
        dayNumber.className = 'calendar-day-number';
        dayNumber.textContent = day;
        dayElement.appendChild(dayNumber);
        
        // Bu tarihin tam halinı oluştur
        const fullDate = new Date(currentDate.getFullYear(), currentDate.getMonth(), day);
        fullDate.setHours(0, 0, 0, 0);
        const dateString = fullDate.toISOString().split('T')[0];
        
        // Geçmiş tarih kontrolü
        if (fullDate < today) {
            dayElement.classList.add('past');
        } else {
            // Bu tarihte slot var mı kontrol et
            const hasSlots = availableSlots.some(slot => slot.date === dateString);
            
            if (hasSlots) {
                dayElement.classList.add('available');
                availableDaysCount++;
                
                // İndikatör ekle
                const indicator = document.createElement('div');
                indicator.className = 'calendar-day-indicator';
                dayElement.appendChild(indicator);
                
                // Tıklanabilir yap
                dayElement.addEventListener('click', function() {
                    console.log('Tarih seçildi:', dateString);
                    selectDate(dateString);
                });
            }
        }
        
        calendar.appendChild(dayElement);
    }
    
    console.log(`Bu ayda ${availableDaysCount} müsait gün var.`);
    
    // Eğer hiç müsait gün yoksa bilgi mesajı göster
    if (availableDaysCount === 0 && availableSlots.length > 0) {
        showNoSlotsMessage('Bu ayda müsait gün bulunmuyor. Lütfen diğer aylara göz atın.');
    } else if (availableSlots.length === 0) {
        showNoSlotsMessage('Şu anda müsait zaman dilimi bulunmamaktadır. Lütfen daha sonra tekrar kontrol edin veya doğrudan iletişime geçin.');
    }
}

// Bilgi mesajı göster
function showNoSlotsMessage(message) {
    const container = document.querySelector('.calendar-container');
    let messageDiv = document.getElementById('noSlotsMessage');
    
    if (!messageDiv) {
        messageDiv = document.createElement('div');
        messageDiv.id = 'noSlotsMessage';
        messageDiv.style.cssText = `
            background: #FFF8F0;
            padding: 1.5rem;
            border-radius: 12px;
            margin-top: 2rem;
            text-align: center;
            border-left: 4px solid #FF6B35;
        `;
        container.appendChild(messageDiv);
    }
    
    messageDiv.innerHTML = `
        <p style="margin: 0; color: #2C3E50;">
            <i class="fas fa-info-circle" style="color: #FF6B35; margin-right: 0.5rem;"></i>
            ${message}
        </p>
        <div style="margin-top: 1rem;">
            <a href="https://wa.me/905078089628" target="_blank" style="color: #FF6B35; font-weight: 600; text-decoration: none;">
                <i class="fab fa-whatsapp"></i> WhatsApp ile İletişime Geç
            </a>
        </div>
    `;
}

// Tarih seçildiğinde
function selectDate(dateString) {
    console.log('selectDate çağrıldı:', dateString);
    const slotsForDate = availableSlots.filter(slot => slot.date === dateString);
    
    console.log('Bu tarih için slot sayısı:', slotsForDate.length);
    
    if (slotsForDate.length === 0) {
        alert('Bu tarih için müsait saat bulunamadı.');
        return;
    }
    
    // Zaman dilimlerini göster
    const timeSlotsContainer = document.getElementById('timeSlotsContainer');
    const timeSlotsGrid = document.getElementById('timeSlots');
    
    if (!timeSlotsContainer || !timeSlotsGrid) {
        console.error('Zaman dilimleri container bulunamadı!');
        return;
    }
    
    timeSlotsGrid.innerHTML = '';
    
    // Tarihi güzel formatta göster
    const dateObj = new Date(dateString);
    const formattedDate = `${dateObj.getDate()} ${monthNames[dateObj.getMonth()]} ${dateObj.getFullYear()}`;
    
    // Başlığı güncelle
    const header = timeSlotsContainer.querySelector('.time-slots-header h4');
    if (header) {
        header.innerHTML = `<i class="fas fa-clock"></i> ${formattedDate} - Müsait Saatler`;
    }
    
    slotsForDate.forEach(slot => {
        const slotElement = document.createElement('div');
        slotElement.className = 'time-slot';
        slotElement.dataset.slotId = slot.id;
        
        const timeDiv = document.createElement('div');
        timeDiv.className = 'time-slot-time';
        timeDiv.textContent = `${slot.start_time} - ${slot.end_time}`;
        
        const typeDiv = document.createElement('div');
        typeDiv.className = 'time-slot-type';
        typeDiv.textContent = slot.lesson_type === 'both' ? 'Online / Yüz Yüze' : 
                              slot.lesson_type === 'online' ? 'Online' : 'Yüz Yüze';
        
        slotElement.appendChild(timeDiv);
        slotElement.appendChild(typeDiv);
        
        slotElement.addEventListener('click', function(e) {
            e.stopPropagation();
            console.log('Slot tıklandı:', slot);
            selectTimeSlot(slot, slotElement);
        });
        
        timeSlotsGrid.appendChild(slotElement);
    });
    
    // Önceki mesajı temizle
    const noSlotsMsg = document.getElementById('noSlotsMessage');
    if (noSlotsMsg) {
        noSlotsMsg.remove();
    }
    
    timeSlotsContainer.classList.add('active');
    
    // Smooth scroll
    setTimeout(() => {
        timeSlotsContainer.scrollIntoView({ behavior: 'smooth', block: 'nearest' });
    }, 100);
}

// Zaman dilimi seçildiğinde
function selectTimeSlot(slot, slotElement) {
    console.log('selectTimeSlot çağrıldı:', slot);
    selectedSlot = slot;
    
    // Tüm slotların seçimini kaldır
    document.querySelectorAll('.time-slot').forEach(el => {
        el.classList.remove('selected');
    });
    
    // Seçilen slotu işaretle
    if (slotElement) {
        slotElement.classList.add('selected');
    }
    
    // Formu göster
    const reservationForm = document.getElementById('reservationForm');
    if (!reservationForm) {
        console.error('Rezervasyon formu bulunamadı!');
        return;
    }
    
    reservationForm.classList.add('active');
    
    // Seçilen bilgileri göster
    const dateObj = new Date(slot.date);
    const dateStr = `${dateObj.getDate()} ${monthNames[dateObj.getMonth()]} ${dateObj.getFullYear()}`;
    
    const selectedDateDisplay = document.getElementById('selectedDateDisplay');
    const selectedTimeDisplay = document.getElementById('selectedTimeDisplay');
    const selectedSlotId = document.getElementById('selectedSlotId');
    
    if (selectedDateDisplay) {
        selectedDateDisplay.textContent = dateStr;
    }
    
    if (selectedTimeDisplay) {
        selectedTimeDisplay.textContent = `${slot.start_time} - ${slot.end_time}`;
    }
    
    if (selectedSlotId) {
        selectedSlotId.value = slot.id;
        console.log('Slot ID set edildi:', slot.id);
    }
    
    // Smooth scroll
    setTimeout(() => {
        reservationForm.scrollIntoView({ behavior: 'smooth', block: 'start' });
    }, 100);
}

// Form gönderildiğinde
const finalForm = document.getElementById('finalReservationForm');
if (finalForm) {
    finalForm.addEventListener('submit', function(e) {
        if (!selectedSlot) {
            e.preventDefault();
            alert('Lütfen bir tarih ve saat seçin.');
            return false;
        }
        
        const slotId = document.getElementById('selectedSlotId').value;
        if (!slotId) {
            e.preventDefault();
            alert('Seçilen zaman diliminde bir hata oluştu. Lütfen tekrar seçin.');
            return false;
        }
        
        console.log('Form gönderiliyor, Slot ID:', slotId);
        // Form normal şekilde submit olacak
    });
}

