<h3>What is the Saga Pattern?<h3/>
The Saga Pattern is a transactional coordination pattern used to achieve eventual consistency in microservices architecture. When a transaction spans across multiple microservices in a distributed system, the transaction must either complete successfully in its entirety, or be rolled back in the event of a failure. Instead of using centralized approaches like two-phase commit (2PC), this pattern allows each microservice to handle its own operations independently and, if needed, execute compensating actions to maintain consistency.
A Saga typically consists of a sequence of steps (or local transactions), with each step being processed by a microservice. If a step fails, compensating transactions are triggered to undo the work performed by the previous steps, thereby ensuring consistency across the system. The Saga Pattern is particularly effective for achieving eventual consistency in large, distributed, and event-driven architectures.
<br><br/>
<h3>Usage of Saga Pattern in the Project</h3>
  <br><br/>
In this project, I implemented the Saga Pattern to maintain consistency across a distributed system. Each business process consists of multiple steps, each handled by a different microservice. The success or failure of each step is monitored, and if any step fails, compensating actions are initiated to revert the previous steps and restore consistency.
<br><br/>
Specifically, the Saga Pattern is used in processes such as Stock Reservation and Payment to orchestrate the following workflows:
<ul>
  <li>Order Creation: When an order is created, the stock is reserved.</li>
  <li>Payment Process: Once the payment is successfully completed, the reserved stock is confirmed.</li>
  <li>Failure Handling: If an error occurs during payment, the reserved stock is released to prevent inconsistencies.</li>
</ul>
<br><br/>
<br><br/>
<h3>Saga Pattern Nedir?</h3>
Saga Pattern, mikrosistemler mimarisinde eventual consistency (nihai tutarlılık) sağlamak amacıyla kullanılan bir işlemsel koordinasyon desenidir. Dağıtık sistemlerde bir işlemin birden fazla mikrosistemi içermesi durumunda, bu işlem ya tamamen başarılı olmalı ya da bir hata durumunda geri alınmalıdır. Bu desen, geleneksel iki aşamalı commit (2PC) gibi merkezi yöntemler yerine, her mikroservisin işlemlerini bağımsız bir şekilde işleyip gerektiğinde telafi edici (compensating) işlemleri yürütmesine olanak tanır.
<br><br/>
Bir Saga, genellikle bir dizi adım (ya da yerel işlem) olarak yapılandırılır ve her adım bir mikroserviste yürütülür. Eğer bir adım başarısız olursa, bu noktaya kadar tamamlanan işlemler geri alınarak sistemde tutarlılık sağlanır. Saga Pattern, özellikle büyük, dağıtık ve event-driven mimarilerde eventual consistency sağlamak için güçlü bir çözümdür.
<h3>Projede Saga Pattern Kullanımı</h3>
<br><br/>
Bu projede, Saga Pattern kullanarak dağıtık sistemde tutarlılığı sağladım. Her bir iş süreci, birden fazla mikroservisin dahil olduğu adımlardan oluşmaktadır. Her adım bir mikroservis tarafından gerçekleştirilirken, işlemin başarılı olup olmadığı izlenmektedir. Eğer herhangi bir adım başarısız olursa, önceki adımları telafi eden işlemler başlatılarak sistemin tutarlı bir duruma geri dönmesi sağlanır.
<br><br/>
Özellikle, bu projede Stock Reservation ve Payment gibi işlem süreçlerinde Saga Pattern'i kullanarak şu iş akışlarını koordine ettim:
<ul>
  <li>Sipariş oluşturma: Sipariş oluşturulduğunda stoklar rezerve edilir.</li>
  <li>Ödeme süreci: Ödeme başarıyla tamamlandığında rezerv edilen stoklar onaylanır.</li>
  <li>Hata durumu yönetimi: Eğer ödeme sırasında bir hata meydana gelirse, rezerve edilen stoklar geri alınarak sistemde tutarsızlık önlenir.</li>
</ul>
