(function($) {
    "use strict";
    var wind = $(window);
    $(function() {
        var akpk = {
            preloader: function() {
                // preloader
                $('#preloader').delay(1000).fadeOut("slow");

            },

            backtotop: function() {
                // Back to top button
                $(window).scroll(function() {
                    if ($(this).scrollTop() > 100) {
                        $('.back-to-top').fadeIn('slow');
                    } else {
                        $('.back-to-top').fadeOut('slow');
                    }
                });
                $('.back-to-top').click(function() {
                    $('html, body').animate({ scrollTop: 0 }, 1500, 'easeInOutExpo');
                    return false;
                });

            },

            headerFixed: function() {
                // Header fixed on scroll
                $(window).scroll(function() {
                    if ($(this).scrollTop() > 100) {
                        $('#header').addClass('header-scrolled');
                    } else {
                        $('#header').removeClass('header-scrolled');
                    }
                });

                if ($(window).scrollTop() > 100) {
                    $('#header').addClass('header-scrolled');
                }

            },

            introHeight: function() {
                // Real view height for mobile devices
                if (window.matchMedia("(max-width: 767px)").matches) {
                    $('#intro').css({ height: $(window).height() });
                }
            },

            navMenuMobile: function() {
                // Mobile Navigation
                if ($('#nav-menu-container').length) {
                    var $mobile_nav = $('#nav-menu-container').clone().prop({
                        id: 'mobile-nav'
                    });
                    $mobile_nav.find('> ul').attr({
                        'class': '',
                        'id': ''
                    });
                    $('body').append($mobile_nav);
                    $('body').prepend('<button type="button" id="mobile-nav-toggle"><i class="fa fa-bars"></i></button>');
                    $('body').append('<div id="mobile-body-overly"></div>');
                    $('#mobile-nav').find('.menu-has-children').prepend('<i class="fa fa-chevron-down"></i>');

                    $(document).on('click', '.menu-has-children i', function(e) {
                        $(this).next().toggleClass('menu-item-active');
                        $(this).nextAll('ul').eq(0).slideToggle();
                        $(this).toggleClass("fa-chevron-up fa-chevron-down");
                    });

                    $(document).on('click', '#mobile-nav-toggle', function(e) {
                        $('body').toggleClass('mobile-nav-active');
                        $('#mobile-nav-toggle i').toggleClass('fa-times fa-bars');
                        $('#mobile-body-overly').toggle();
                    });

                    $(document).click(function(e) {
                        var container = $("#mobile-nav, #mobile-nav-toggle");
                        if (!container.is(e.target) && container.has(e.target).length === 0) {
                            if ($('body').hasClass('mobile-nav-active')) {
                                $('body').removeClass('mobile-nav-active');
                                $('#mobile-nav-toggle i').toggleClass('fa-times fa-bars');
                                $('#mobile-body-overly').fadeOut();
                            }
                        }
                    });
                } else if ($("#mobile-nav, #mobile-nav-toggle").length) {
                    $("#mobile-nav, #mobile-nav-toggle").hide();
                }

            },

            smoothScroll: function() {
                $('.nav-menu a, #mobile-nav a, .scrollto').on('click', function() {
                    if (location.pathname.replace(/^\//, '') == this.pathname.replace(/^\//, '') && location.hostname == this.hostname) {
                        var target = $(this.hash);
                        if (target.length) {
                            var top_space = 0;

                            if ($('#header').length) {
                                top_space = $('#header').outerHeight();

                                if (!$('#header').hasClass('header-fixed')) {
                                    top_space = top_space - 20;
                                }
                            }

                            $('html, body').animate({
                                scrollTop: target.offset().top - top_space
                            }, 1500, 'easeInOutExpo');

                            if ($(this).parents('.nav-menu').length) {
                                $('.nav-menu .menu-active').removeClass('menu-active');
                                $(this).closest('li').addClass('menu-active');
                            }

                            if ($('body').hasClass('mobile-nav-active')) {
                                $('body').removeClass('mobile-nav-active');
                                $('#mobile-nav-toggle i').toggleClass('fa-times fa-bars');
                                $('#mobile-body-overly').fadeOut();
                            }
                            return false;
                        }
                    }
                });

            },

            accMobile: function() {
                // accordion footer on mobile devices
                var accHeader = $('.footer-box h6');

                if (wind.width() <= 991) {
                    accHeader.append('<span class="acc-arrow fas fa-chevron-down"></span>');
                    $('.footer-box h6').click(function() {
                        $(this).parent().toggleClass("active");
                        if ($(this).parent().hasClass('active')) {
                            $(this).find('.acc-arrow').removeClass('fa-chevron-down').addClass('fa-chevron-up');
                        } else {
                            $(this).find('.acc-arrow').removeClass('fa-chevron-up').addClass('fa-chevron-down');
                        }
                    });
                } else {
                    $('.acc-arrow').css({
                        'display': 'none'
                    });
                }

            },

            testimonialSlide: function() {
                // Slick Slider
                $('.testimonial-slide').slick({
                    speed: 800,
                    slidesToShow: 1,
                    slidesToScroll: 1,
                    autoplay: true,
                    autoplaySpeed: 6000
                });
            },

            learningSlide: function() {
                //learning centre slide
                $(".owl-learning").owlCarousel({
                    items: 3,
                    margin: 20,
                    loop: false,
                    responsive: {
                        0: {
                            items: 1
                        },
                        576: {
                            items: 1
                        },
                        768: {
                            items: 2
                        },
                        992: {
                            items: 2
                        },
                        1200: {
                            items: 3
                        }
                    }
                });

            },

            learningStep: function() {
                //add class active when click on step
                var learningStep = $(".elementor-timeline-carousel .timeline-carosuel-item .timeline-content");
                learningStep.click(function() {
                    $(".elementor-timeline-carousel .timeline-carosuel-item .timeline-content").removeClass("active");
                    $(this).addClass("active");
                });
            },

            newsSlide: function() {
                $('.owl-news').slick({
                    speed: 800,
                    slidesToShow: 1,
                    variableWidth: true,
                    slidesToScroll: 1,
                    autoplay: true,
                    autoplaySpeed: 6000,
                    responsive: [{
                            breakpoint: 992,
                            settings: {
                                variableWidth: false,
                                slidesToShow: 2
                            }
                        },
                        {
                            breakpoint: 480,
                            settings: {
                                variableWidth: false,
                                slidesToShow: 1
                            }
                        }
                    ]
                });

                //positioning the arrow button
                $(".news-carousel button").wrapAll("<div class='news-button'></div>");
                $(".news-button").appendTo(".news-desc");
            },

            tooltip: function() {
                $('[data-toggle="tooltip"]').tooltip();
            },

            //configure the select2 custom plugin
            selectOption: function() {
                $('.select2').select2({
                    minimumResultsForSearch: Infinity
                });

                $('.select2-area').select2({
                    placeholder: 'Select Area'
                });

            },

            countToStat: function() {
                $('.counter-count').countTo({
                    refreshInterval: 50,
                    speed: 8000,
                    // decimals: 2
                });

            },

            toggleMenu: function() {
                $(".other-menu-wrap").on('show.bs.dropdown', function() {
                    $(this).find(".wrapper-menu").addClass("open");
                }).on('hide.bs.dropdown', function() {
                    $(this).find(".wrapper-menu").removeClass("open");
                });

            },

            parallaxImage: function() {
                var rellax = new Rellax('.rellax', {
                    center: true,
                    wrapper: '#main'
                });
            },

            shareButton: function() {
                $(".share-wrap .social").jsSocials({
                    showLabel: false,
                    showCount: false,
                    shares: ["facebook", "twitter", "email"]
                });


                $(".share-icon").click(function(e) {
                    $('.share-wrap .social').not($(this).next(".share-wrap .social")).each(function() {
                        $(this).removeClass("active");
                    });

                    $(this).next(".share-wrap .social").toggleClass("active");
                });

                $(".mediasocial").jsSocials({
                    showLabel: false,
                    showCount: false,
                    shares: ["facebook", "twitter", "linkedin"]
                });

            },

            //append active breadcrumb to title
            activeBread: function() {
                var activeBread = $('#breadcrumb .breadcrumb li:last-child > a');
                activeBread.appendTo('h1.page-title');
            },

            datePick: function() {
                $(".pick-date").flatpickr({
                    mode: "range",
                    dateFormat: "j M y",
                    wrap: true,
                    locale: {
                        rangeSeparator: ' - '
                    }
                });
            },

            activeTab: function() {
                $('.akpk-tab li.nav-item:first-child').addClass('active');
                $('.akpk-tab li.nav-item a').on('click', function() {

                    $(".akpk-tab li.nav-item").removeClass('active');
                    $(this).parent().addClass('active');

                });

            },

            cardSlide: function() {
                $(".card-slider").owlCarousel({
                    items: 4,
                    loop: false,
                    responsive: {
                        0: {
                            items: 1
                        },
                        576: {
                            items: 1
                        },
                        768: {
                            items: 2
                        },
                        992: {
                            items: 2
                        },
                        1200: {
                            items: 4
                        }
                    }
                });

            }


        };

        akpk.preloader();
        akpk.backtotop();
        akpk.headerFixed();
        akpk.introHeight();
        akpk.navMenuMobile();
        akpk.smoothScroll();
        akpk.accMobile();
        akpk.testimonialSlide();
        akpk.learningSlide();
        akpk.learningStep();
        akpk.newsSlide();
        akpk.tooltip();
        akpk.selectOption();
        akpk.countToStat();
        akpk.toggleMenu();
        akpk.parallaxImage();
        akpk.activeBread();
        akpk.datePick();
        akpk.shareButton();
        akpk.activeTab();
        akpk.cardSlide();

    });

    // Initiate the wowjs animation library
    new WOW().init();


    //Show more content
    $('.some-list').simpleLoadMore({
        item: '.card__wrapper',
        count: 6,
        btnHTML: '<div class="text-center"><a href="#" class="load-more__btn btn btn-outline-secondary btn-rounded">SHOW MORE</a></div>'
    });




})(jQuery);