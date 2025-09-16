import { CommonModule } from '@angular/common';
import { Component, Input, OnChanges, SimpleChanges, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { TodoItem, UpdateTodoItem } from '../../models/todo-item.model';
import { TodoService } from '../../services/todo.service';

@Component({
  selector: 'app-todo-list',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatCheckboxModule,
    MatButtonModule,
    MatIconModule,
    MatDividerModule,
    MatListModule,
    MatSnackBarModule
  ],
  template: `
    <mat-card class="mt-4">
      <mat-card-header>
        <mat-card-title>My Tasks</mat-card-title>
      </mat-card-header>
      <mat-card-content>
        <mat-list>
          <ng-container *ngIf="todos.length; else noTodos">
            <mat-list-item *ngFor="let todo of todos">
              <div class="todo-item">
                <mat-checkbox
                  [checked]="todo.isCompleted"
                  (change)="toggleComplete(todo)"
                  color="primary">
                  <span [class.completed]="todo.isCompleted">{{ todo.title }}</span>
                </mat-checkbox>
                <button mat-icon-button color="warn" (click)="deleteTodo(todo.id)">
                  <mat-icon>delete</mat-icon>
                </button>
              </div>
            </mat-list-item>
          </ng-container>
          
          <ng-template #noTodos>
            <p class="no-todos">No tasks yet. Add one above!</p>
          </ng-template>
        </mat-list>
      </mat-card-content>
    </mat-card>
  `,
})
export class TodoListComponent implements OnChanges {
  @Input() refresh = false;

  private todoService = inject(TodoService);
  private snackBar = inject(MatSnackBar);

  todos: TodoItem[] = [];

  ngOnInit(): void {
    this.loadTodos();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['refresh']) {
      this.loadTodos();
    }
  }

  loadTodos(): void {
    this.todoService.getTodos().subscribe({
      next: (todos) => {
        this.todos = todos;
      },
      error: (error) => {
        console.error('Error loading todos:', error);
        this.snackBar.open('Error loading tasks', 'Close', {
          duration: 3000
        });
      }
    });
  }

  toggleComplete(todo: TodoItem): void {
    const updateData: UpdateTodoItem = {
      title: todo.title,
      isCompleted: !todo.isCompleted
    };

    this.todoService.updateTodo(todo.id, updateData).subscribe({
      next: () => {
        todo.isCompleted = !todo.isCompleted;
      },
      error: (error) => {
        console.error('Error updating todo:', error);
        this.snackBar.open('Error updating task', 'Close', {
          duration: 3000
        });
      }
    });
  }

  deleteTodo(id: string): void {
    this.todoService.deleteTodo(id).subscribe({
      next: () => {
        this.todos = this.todos.filter(todo => todo.id !== id);
        this.snackBar.open('Task deleted', 'Close', {
          duration: 3000
        });
      },
      error: (error) => {
        console.error('Error deleting todo:', error);
        this.snackBar.open('Error deleting task', 'Close', {
          duration: 3000
        });
      }
    });
  }
}
