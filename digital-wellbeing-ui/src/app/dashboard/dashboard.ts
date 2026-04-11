import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UsageService } from '../services/usage';
import Chart from 'chart.js/auto';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.html'
})
export class Dashboard implements OnInit {

  constructor(private usageService: UsageService) { }

  ngOnInit() {
    this.loadChart();
  }

  loadChart() {
    this.usageService.getSummary().subscribe(data => {

      console.log("API Data:", data); // 👈 debug

      const labels = data.map(x => x.appName);
      const values = data.map(x => x.totalTime);

      new Chart("usageChart", {
        type: 'bar',
        data: {
          labels: labels,
          datasets: [{
            label: 'Time Spent (seconds)',
            data: values
          }]
        }
      });
    });
  }
}
