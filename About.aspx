<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="Checkmate.com2.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.3/css/all.min.css">
    <link href="Style2.css" rel="stylesheet" />
    <main class="container mt-5">
        <section>
            <div class="text-center">
                <h1 class="display-4">About Us</h1>
                <p class="lead">Discover our story, mission, and the people behind Checkmate Bakery.</p>
            </div>

            <div class="row mt-5">
                <div class="col-md-6 georgia-font">
                    <h2>Our Story</h2>
                    <p>In the heart of a vibrant town, Checkmate Bakery emerged as a dream realized. It all began when Thembeka, an avid chess enthusiast and passionate baker, decided to merge her two passions into a bakery unlike any other. Her journey was defined by strategic planning and an unwavering commitment to crafting exceptional pastries. Sarah's chess-inspired bakery was a checkmate move in itself. With each delectable creation, she cleverly fused the precision of chess with the artistry of baking. The result was a bakery that offered not only delightful treats but also a unique and engaging experience for her customers.</p>
                </div>
                <div class="col-md-6">
                    <img src="images/display.jpg" alt="About Us" class="img-fluid rounded">
                </div>
            </div>
        </section>
        <section class="management">
            <div class="menu-item">
                <h2>Management</h2>
                <p>Our dedicated management team brings a wealth of experience and a passion for excellence to Checkmate Bakery. Meet the individuals who lead us towards sweet victories:</p>
            </div>
        </section>
       
        <section class="menu-grid">
            <%--<div class="menu-item">
                <h2>Management</h2>
                <p>Our dedicated management team brings a wealth of experience and a passion for excellence to Checkmate Bakery. Meet the individuals who lead us towards sweet victories:</p>
            </div>--%>
            <div class="menu-item card">
                <img src="/images/mthe.jpg" alt="Thembeka Mchunu">
                <h1>Thembeka Mchunu</h1>
                <p class="title">CEO & Founder</p>
                <p>University Of KwaZulu-Natal</p>
                <div class="social-icons">
                    <a href="#"><i class="fa fa-dribbble"></i></a>
                    <a href="#"><i class="fa fa-twitter"></i></a>
                    <a href="#"><i class="fa fa-linkedin"></i></a>
                    <a href="#"><i class="fa fa-facebook"></i></a>
                </div>
                <button>Contact</button>
            </div>
            <div class="menu-item card">
                <img src="/images/sanele.jpg" alt="Sanele Mchunu">
                <h1>Sanele Mchunu</h1>
                <p class="title">Head Baker</p>
                <p>University Of Limpopo</p>
                <div class="social-icons">
                    <a href="#"><i class="fa fa-dribbble"></i></a>
                    <a href="#"><i class="fa fa-twitter"></i></a>
                    <a href="#"><i class="fa fa-linkedin"></i></a>
                    <a href="#"><i class="fa fa-facebook"></i></a>
                </div>
                <button>Contact</button>
            </div>
            <div class="menu-item card">
                <img src="/images/Ntobeko.jpg" alt="Thethwayo Mchunu">
                <h1>Thethwayo Mchunu</h1>
                <p class="title">Chief Operations Officer</p>
                <p>University Of Cape Town</p>
                <div class="social-icons">
                    <a href="#"><i class="fa fa-dribbble"></i></a>
                    <a href="#"><i class="fa fa-twitter"></i></a>
                    <a href="#"><i class="fa fa-linkedin"></i></a>
                    <a href="#"><i class="fa fa-facebook"></i></a>
                </div>
                <button>Contact</button>
            </div>
        </section>


        <section class="quality">
            <div class="quality-content">
                <div class="quality-text">
                    <h2>Quality Assurance</h2>
                    <p>At Checkmate Bakery, quality is our top priority. We take pride in delivering pastries that not only taste heavenly but also meet the highest standards of freshness and quality. Our quality assurance process includes:</p>
                    <ul>
                        <li>Hand-selecting the finest ingredients</li>
                        <li>Regular quality checks at every stage of production</li>
                        <li>Customer feedback and satisfaction surveys</li>
                    </ul>
                </div>
                <div class="quality-image">
                    <img src="images/login1.jpeg" alt="Quality Assurance" class="img-fluid">
                </div>
            </div>
        </section>
        <!-- ... (unchanged) ... -->
        <section class="mission">
            <div class="mission-content">
                <div class="mission-text">
                    <h2>Mission</h2>
                    <p>At Checkmate Bakery, our mission is simple: We make irresistible pastries that are a winning move for your taste buds. With quality, innovation, and a commitment to our community, we're here to delight you with sweet victories in every bite.</p>
                </div>
                <div class="mission-visual">
                    <img src="images/login3.jpeg" alt="Checkmate Bakery Mission"/>
                    <i class="fa fa-check-circle"></i>
                </div>
            </div>
        </section>
    </main>
    <section class="call-to-action section">
        <div class="container">
            <div class="row">
                <div class="col-md-12 text-center">
                    <h2>Let's Create Something Together</h2>
                    <p>Proin gravida nibh vel velit auctor aliquet. Aenean sollicudin bibendum auctor,
                        <br>
                        nisi elit consequat ipsum, nesagittis sem nid elit. Duis sed odio sitain elit.</p>
                    <a href="Contact.aspx" class="btn btn-main">Contact Us</a>
                </div>
            </div>
            <!-- End row -->
        </div>
        <!-- End container -->
    </section>   <!-- End section -->
      <!-- Footer -->
   <footer class="site-footer">
        <div class="footer-content">
            <!-- Quick Links Section -->
            <div class="footer-section">
                <h3>Quick Links</h3>
                <ul class="footer-icons">
                    <li><a href="~/"><i class="fas fa-home"></i>Home</a></li>
                    <li><a href="~/Shop.aspx"><i class="fas fa-utensils"></i>Menu</a></li>
                    <li><a href="~/Shop.aspx"><i class="fas fa-shopping-cart"></i>Order Now</a></li>
                    <li><a href="~/Contact.aspx"><i class="fas fa-phone"></i>Contact Us</a></li>
                </ul>
            </div>

            <!-- Contact Info Section -->
            <div class="footer-section">
                <h3>Contact Info</h3>
                <p>Email: info@checkmate12.com</p>
                <p>Phone: (+27)78 219 9105</p>
                <p>Address: University Rd, Westville, Durban</p>
            </div>

            <!-- Social Media Section -->
            <div class="footer-section">
                <h3>Follow Us</h3>
                <ul class="social-icons">
                    <li><a href="#"><i class="fab fa-facebook"></i></a></li>
                    <li><a href="#"><i class="fab fa-twitter"></i></a></li>
                    <li><a href="#"><i class="fab fa-instagram"></i></a></li>
                    <li><a href="#"><i class="fab fa-linkedin"></i></a></li>
                </ul>
            </div>
        </div>
    </footer>   
    <link href="https://fonts.googleapis.com/css2?family=Oswald&display=swap" rel="stylesheet">
</asp:Content>
