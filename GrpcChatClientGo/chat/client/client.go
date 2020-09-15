package main

import (
    "bufio"
    "fmt"
    "log"
    "os"
    "golang.org/x/net/context"
    pb "chat"
    "google.golang.org/grpc"
    "google.golang.org/grpc/credentials"
)

func listen(stream pb.ChatRoom_JoinClient, inbox chan pb.Message) {
    for {
        msg, _ := stream.Recv()
        inbox <- *msg
    }
}

func send(outbox chan pb.Message, r *bufio.Scanner, user string) {
    for r.Scan() {
        txt := r.Text()
        outbox <- pb.Message{User: user, Text: txt}
    }
}

func main() {
    r := bufio.NewScanner(os.Stdin)

    address := "localhost:5001"
    creds := credentials.NewClientTLSFromCert(nil, "")
    conn, err := grpc.Dial(address, grpc.WithTransportCredentials(creds))
    if err != nil {
        log.Fatalf("Could not connect: %v", err)
    }

    defer conn.Close()

    c := pb.NewChatRoomClient(conn)

    fmt.Printf("[INFO] input your user name: ")
    r.Scan()
    me := r.Text()

    stream, err := c.Join(context.Background())
    if err != nil {
        fmt.Println(err)
        return
    }

    inbox := make(chan pb.Message, 1000)
    go listen(stream, inbox)

    outbox := make(chan pb.Message, 1000)
    go send(outbox, r, me)

    stream.Send(&pb.Message{User: me, Text: fmt.Sprintf("[INFO] %s has joined the room.", me)})

    for {
        select {
        case sending := <- outbox:
            stream.Send(&sending)
        case received := <- inbox:
            fmt.Printf("[%s] %s\n", received.User, received.Text)
        }
    }
}