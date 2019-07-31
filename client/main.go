package main

import (
	"bufio"
	"context"
	"fmt"
	"log"
	"os"
	"time"

	"google.golang.org/grpc"
	pb "puppyloper.com/grpc-client/service"
)

const (
	address = "localhost:50001"
)

func main() {
	conn, err := grpc.Dial(address, grpc.WithInsecure())
	if err != nil {
		log.Fatalf("did not connect: %v", err)
	}
	defer conn.Close()

	c := pb.NewGameServiceClient(conn)
	scanner := bufio.NewScanner(os.Stdin)
	stop := false
	count := 1
	for !stop {
		fmt.Print("Enter command (create, get, getAll, stop) : ")
		scanner.Scan()
		text := scanner.Text()
		switch text {
		case "create":
			g := &pb.Game{
				GameCode:   int32(10000 + count),
				Name:       fmt.Sprintf("Game %v", count),
				Platform:   pb.Game_PC,
				LandingUrl: fmt.Sprintf("https://game.com/%v", count),
				ImageUrl:   fmt.Sprintf("https://image.com/game-images/%v", count),
			}
			count++
			ctx, cancel := createContext()
			defer cancel()
			r, err := c.CreateGame(ctx, &pb.CreateGameRequest{Game: g})
			if err != nil {
				log.Fatalf("could not create game: %v", err)
			}
			log.Printf("Created game: %v", r)
		case "get":
			ctx, cancel := createContext()
			defer cancel()
			r, err := c.GetGame(ctx, &pb.GetGameRequest{Id: 2})
			if err != nil {
				log.Fatalf("could not get game: %v", err)
			}
			log.Printf("Get game: %v", r)
		case "getAll":
			ctx, cancel := createContext()
			defer cancel()
			r, err := c.GetGames(ctx, &pb.GetGamesRequest{})
			if err != nil {
				log.Fatalf("could not get games: %v", err)
			}
			log.Printf("Get games: %v", r)
		case "stop":
			stop = true
		}
	}
}

func createContext() (context.Context, context.CancelFunc) {
	return context.WithTimeout(context.Background(), time.Second)
}
