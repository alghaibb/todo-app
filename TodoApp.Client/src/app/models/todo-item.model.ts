export interface TodoItem {
  id: string;
  title: string;
  isCompleted: boolean;
  createdAt: string;
}

export interface CreateTodoItem {
  title: string;
}

export interface UpdateTodoItem {
  title: string;
  isCompleted: boolean;
}
