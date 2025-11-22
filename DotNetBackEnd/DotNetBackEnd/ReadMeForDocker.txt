in folderul cu docker-compose.yml (merge si in pmc):

	docker compose up -d
	docker ps ==> dotnetbackend-kafka-1
				  dotnetbackend-zookeeper-1

Test-NetConnection -Port 9092 -ComputerName localhost  => TcpTestSucceeded : True

in cmd: 

docker exec -it dotnetbackend-kafka-1 bash
kafka-topics --create --topic order-created --bootstrap-server localhost:9092 --partitions 1 --replication-factor 1
kafka-topics --create --topic payment-authorized --bootstrap-server localhost:9092 --partitions 1 --replication-factor 1
kafka-topics --create --topic inventory-reserved --bootstrap-server localhost:9092 --partitions 1 --replication-factor 1
kafka-topics --create --topic order-completed --bootstrap-server localhost:9092 --partitions 1 --replication-factor 1


kafka-topics --list --bootstrap-server localhost:9092
 ==> order-created
	payment-authorized
	inventory-reserved
	order-completed



docker compose down --volumes //sterge volumele create pt kafka
docker compose down //doar opreste containerele

docker stop dotnetbackend-kafka-1 
docker stop dotnetbackend-zookeeper-1
