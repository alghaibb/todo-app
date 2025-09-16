import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatToolbarModule } from '@angular/material/toolbar';
import { TodoFormComponent } from './components/todo-form/todo-form.component';
import { TodoListComponent } from './components/todo-list/todo-list.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatToolbarModule,
    TodoFormComponent,
    TodoListComponent
  ],
  template: `
    <mat-toolbar color="primary">
      <span>Todo App</span>
    </mat-toolbar>
    <div class="container">
      <mat-card class="mt-4">
        <mat-card-header>
          <mat-card-title>Add New Task</mat-card-title>
        </mat-card-header>
        <mat-card-content>
          <app-todo-form (todoAdded)="handleTodoAdded()"></app-todo-form>
        </mat-card-content>
      </mat-card>
      
      <app-todo-list [refresh]="refresh"></app-todo-list>
    </div>
  `,
})
export class AppComponent {
  refresh = false;

  handleTodoAdded() {
    this.refresh = !this.refresh;
  }
}
