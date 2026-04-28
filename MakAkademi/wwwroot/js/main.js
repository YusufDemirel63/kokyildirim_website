// ============================================
// Mobile Navigation Toggle
// ============================================
document.addEventListener('DOMContentLoaded', function() {
    const navToggle = document.getElementById('navToggle');
    const navMenu = document.getElementById('navMenu');
    
    if (navToggle && navMenu) {
        navToggle.addEventListener('click', function() {
            navMenu.classList.toggle('active');
            
            // Animate hamburger icon
            const spans = navToggle.querySelectorAll('span');
            spans[0].style.transform = navMenu.classList.contains('active') ? 'rotate(45deg) translate(5px, 5px)' : '';
            spans[1].style.opacity = navMenu.classList.contains('active') ? '0' : '1';
            spans[2].style.transform = navMenu.classList.contains('active') ? 'rotate(-45deg) translate(7px, -6px)' : '';
        });
        
        // Close menu when clicking outside
        document.addEventListener('click', function(e) {
            if (!navToggle.contains(e.target) && !navMenu.contains(e.target)) {
                navMenu.classList.remove('active');
                const spans = navToggle.querySelectorAll('span');
                spans[0].style.transform = '';
                spans[1].style.opacity = '1';
                spans[2].style.transform = '';
            }
        });
        
        // Close menu when clicking on a link
        const navLinks = navMenu.querySelectorAll('.nav-link');
        navLinks.forEach(link => {
            link.addEventListener('click', function() {
                navMenu.classList.remove('active');
                const spans = navToggle.querySelectorAll('span');
                spans[0].style.transform = '';
                spans[1].style.opacity = '1';
                spans[2].style.transform = '';
            });
        });
    }
});

// ============================================
// Close Alert Messages
// ============================================
document.addEventListener('DOMContentLoaded', function() {
    const closeButtons = document.querySelectorAll('.close-alert');
    
    closeButtons.forEach(button => {
        button.addEventListener('click', function() {
            const alert = this.parentElement;
            alert.style.animation = 'slideOut 0.3s ease';
            setTimeout(() => {
                alert.remove();
            }, 300);
        });
    });
    
    // Auto-close alerts after 5 seconds
    const alerts = document.querySelectorAll('.alert');
    alerts.forEach(alert => {
        setTimeout(() => {
            alert.style.animation = 'slideOut 0.3s ease';
            setTimeout(() => {
                alert.remove();
            }, 300);
        }, 5000);
    });
});

// ============================================
// Smooth Scroll
// ============================================
document.querySelectorAll('a[href^="#"]').forEach(anchor => {
    anchor.addEventListener('click', function (e) {
        e.preventDefault();
        const target = document.querySelector(this.getAttribute('href'));
        if (target) {
            target.scrollIntoView({
                behavior: 'smooth',
                block: 'start'
            });
        }
    });
});

// ============================================
// Scroll to Top on Page Load
// ============================================
window.addEventListener('beforeunload', function() {
    window.scrollTo(0, 0);
});

// ============================================
// Add animation classes on scroll
// ============================================
const observerOptions = {
    threshold: 0.1,
    rootMargin: '0px 0px -50px 0px'
};

const observer = new IntersectionObserver(function(entries) {
    entries.forEach(entry => {
        if (entry.isIntersecting) {
            entry.target.style.opacity = '1';
            entry.target.style.transform = 'translateY(0)';
        }
    });
}, observerOptions);

// Observe elements for animation
document.addEventListener('DOMContentLoaded', function() {
    const animatedElements = document.querySelectorAll('.feature-card, .lesson-card, .testimonial-card, .method-card, .faq-item, .blog-card');
    
    animatedElements.forEach(el => {
        el.style.opacity = '0';
        el.style.transform = 'translateY(20px)';
        el.style.transition = 'opacity 0.6s ease, transform 0.6s ease';
        observer.observe(el);
    });
});

// ============================================
// Form Validation Enhancement
// ============================================
document.addEventListener('DOMContentLoaded', function() {
    const forms = document.querySelectorAll('form');
    
    forms.forEach(form => {
        const inputs = form.querySelectorAll('.form-control');
        
        inputs.forEach(input => {
            input.addEventListener('blur', function() {
                validateInput(this);
            });
            
            input.addEventListener('input', function() {
                if (this.classList.contains('error')) {
                    validateInput(this);
                }
            });
        });
        
        form.addEventListener('submit', function(e) {
            let isValid = true;
            
            inputs.forEach(input => {
                if (!validateInput(input)) {
                    isValid = false;
                }
            });
            
            if (!isValid) {
                e.preventDefault();
            }
        });
    });
});

function validateInput(input) {
    const value = input.value.trim();
    const type = input.type;
    let isValid = true;
    let errorMessage = '';
    
    // Check if required
    if (input.hasAttribute('required') && value === '') {
        isValid = false;
        errorMessage = 'Bu alan zorunludur.';
    }
    
    // Email validation
    if (type === 'email' && value !== '') {
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailRegex.test(value)) {
            isValid = false;
            errorMessage = 'Geçerli bir e-posta adresi girin.';
        }
    }
    
    // Phone validation (Turkish format)
    if (input.name === 'phone' && value !== '') {
        const phoneRegex = /^[0-9\s\-\+\(\)]{10,}$/;
        if (!phoneRegex.test(value)) {
            isValid = false;
            errorMessage = 'Geçerli bir telefon numarası girin.';
        }
    }
    
    // Show/hide error
    const formGroup = input.closest('.form-group');
    let errorElement = formGroup.querySelector('.form-error');
    
    if (!isValid) {
        input.classList.add('error');
        input.style.borderColor = '#ef4444';
        
        if (!errorElement) {
            errorElement = document.createElement('div');
            errorElement.className = 'form-error';
            formGroup.appendChild(errorElement);
        }
        errorElement.textContent = errorMessage;
    } else {
        input.classList.remove('error');
        input.style.borderColor = '';
        if (errorElement && !input.hasAttribute('data-server-error')) {
            errorElement.remove();
        }
    }
    
    return isValid;
}

// ============================================
// WhatsApp Float Button Animation
// ============================================
document.addEventListener('DOMContentLoaded', function() {
    const whatsappBtn = document.querySelector('.whatsapp-float');
    
    if (whatsappBtn) {
        // Pulse animation
        setInterval(() => {
            whatsappBtn.style.animation = 'pulse 1s ease';
            setTimeout(() => {
                whatsappBtn.style.animation = '';
            }, 1000);
        }, 5000);
    }
});

// Add pulse animation to CSS dynamically
const style = document.createElement('style');
style.textContent = `
    @keyframes pulse {
        0%, 100% {
            transform: scale(1);
        }
        50% {
            transform: scale(1.1);
        }
    }
    
    @keyframes slideOut {
        from {
            transform: translateX(0);
            opacity: 1;
        }
        to {
            transform: translateX(100%);
            opacity: 0;
        }
    }
`;
document.head.appendChild(style);

// ============================================
// FAQ Accordion
// ============================================
document.addEventListener('DOMContentLoaded', function() {
    const accordionHeaders = document.querySelectorAll('.faq-accordion-header');
    const accordionItems = document.querySelectorAll('.faq-accordion-item');

    // Güvenli başlangıç durumu: hepsini kapat (tüm cihazlarda)
    accordionItems.forEach(item => {
        item.classList.remove('active');
        const content = item.querySelector('.faq-accordion-content');
        if (content) {
            content.style.display = 'none';
            content.style.maxHeight = '0px';
            content.style.opacity = '0';
            content.style.visibility = 'hidden';
            content.style.padding = '0';
        }
    });
    
    accordionHeaders.forEach(header => {
        header.addEventListener('click', function() {
            const accordionItem = this.parentElement;
            const isActive = accordionItem.classList.contains('active');

            // Close all
            document.querySelectorAll('.faq-accordion-item').forEach(item => {
                item.classList.remove('active');
                const content = item.querySelector('.faq-accordion-content');
                if (content) {
                    content.style.display = 'none';
                    content.style.maxHeight = '0px';
                    content.style.opacity = '0';
                    content.style.visibility = 'hidden';
                    content.style.padding = '0';
                }
            });

            // Toggle current
            if (!isActive) {
                accordionItem.classList.add('active');
                const content = accordionItem.querySelector('.faq-accordion-content');
                if (content) {
                    content.style.display = 'block';
                    content.style.opacity = '1';
                    content.style.visibility = 'visible';
                    content.style.padding = '1rem 1.5rem 1.5rem 1.5rem';
                    // CSS'deki max-height: 1000px kuralı devreye girecek
                    content.style.maxHeight = '1000px';
                }
            }
        });
    });
});

// ============================================
// Sticky Header on Scroll
// ============================================
let lastScroll = 0;
const header = document.querySelector('.header');

window.addEventListener('scroll', () => {
    const currentScroll = window.pageYOffset;
    
    if (currentScroll > 100) {
        header.style.boxShadow = '0 4px 12px rgba(0, 0, 0, 0.1)';
    } else {
        header.style.boxShadow = '0 4px 6px -1px rgba(0, 0, 0, 0.1)';
    }
    
    lastScroll = currentScroll;
});

// ============================================
// Loading State for Forms
// ============================================
document.addEventListener('DOMContentLoaded', function() {
    const forms = document.querySelectorAll('form');
    
    forms.forEach(form => {
        form.addEventListener('submit', function(e) {
            const submitBtn = this.querySelector('button[type="submit"]');
            if (submitBtn && !submitBtn.disabled) {
                const originalText = submitBtn.innerHTML;
                submitBtn.disabled = true;
                submitBtn.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Gönderiliyor...';
                
                // Re-enable after 3 seconds (fallback)
                setTimeout(() => {
                    submitBtn.disabled = false;
                    submitBtn.innerHTML = originalText;
                }, 3000);
            }
        });
    });
});

// ============================================
// Print Page Functionality (optional)
// ============================================
function printPage() {
    window.print();
}

// ============================================
// Copy to Clipboard (for contact info)
// ============================================
function copyToClipboard(text) {
    navigator.clipboard.writeText(text).then(() => {
        // Show temporary success message
        const message = document.createElement('div');
        message.className = 'alert alert-success';
        message.innerHTML = '<span>Panoya kopyalandı!</span>';
        message.style.position = 'fixed';
        message.style.top = '20px';
        message.style.right = '20px';
        message.style.zIndex = '9999';
        document.body.appendChild(message);
        
        setTimeout(() => {
            message.remove();
        }, 2000);
    });
}

// ============================================
// Initialize all functions
// ============================================
console.log('MAK Matematik - Website loaded successfully!');

