export interface Group{
    name:String;
    connections:Connection[]
}

export interface Connection {
    connectionId:string;
    username:string;
}