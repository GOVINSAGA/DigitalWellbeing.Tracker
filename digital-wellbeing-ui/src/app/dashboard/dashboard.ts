import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UsageService } from '../services/usage';
import Chart from 'chart.js/auto';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.html',
  styleUrls: ['./dashboard.scss']
})
export class Dashboard implements OnInit {

  totalTime = 0;
  topApp = '';

  constructor(private usageService: UsageService) { }

  ngOnInit() {
    this.loadCharts();
  }

  loadCharts() {
    this.usageService.getSummary().subscribe(data => {

      // Sort & limit
      const sorted = data
        .sort((a, b) => b.totalTime - a.totalTime)
        .slice(0, 6);

      // Summary
      this.totalTime = data.reduce((sum, x) => sum + x.totalTime, 0);
      this.topApp = sorted[0]?.appName;

      const labels = sorted.map(x => x.appName);
      const values = sorted.map(x => (x.totalTime / 60).toFixed(2));

      // 🔵 Bar Chart
      new Chart("barChart", {
        type: 'bar',
        data: {
          labels: labels,
          datasets: [{
            label: 'Time (minutes)',
            data: values
          }]
        }
      });

      // 🟣 Pie Chart
      new Chart("pieChart", {
        type: 'pie',
        data: {
          labels: labels,
          datasets: [{
            data: values
          }]
        }
      });
    });
  }
}
