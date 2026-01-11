import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BaseChartDirective }  from 'ng2-charts';
import { SectionCardComponent } from '../../shared/section-card/section-card.component';
import { KpiCardComponent } from '../../shared/kpi-card/kpi-card.component';

@Component({
  selector: 'app-dashboard-user',
  standalone: true,
  imports: [CommonModule, BaseChartDirective, SectionCardComponent, KpiCardComponent],
  templateUrl: './dashboard-user.component.html',
  styleUrl: './dashboard-user.component.scss'
})
export class DashboardUserComponent {
  // mock — înlocuiești cu API mai târziu
  globalKpis = [
    { label: 'Stacks', value: '2', hint: '.NET vs Go' },
    { label: 'Scenarios', value: '3', hint: 'baseline / high-load / fault' },
    { label: 'Metrics', value: '4', hint: 'latency / throughput / cpu / mem' },
    { label: 'Last run', value: '—', hint: 'no data yet' },
  ];

  dotnet = {
    title: '.NET stack',
    subtitle: 'ASP.NET Core + MediatR + Confluent Kafka',
    bullets: [
      'Mediator pattern (in-process dispatch) + Kafka (durable streaming)',
      'Focus: latency under load, handler contention at high concurrency',
      'Profiling: dotnet-trace / counters'
    ],
    kpis: [
      { label: 'P95', value: '42 ms', hint: 'baseline' },
      { label: 'P99', value: '75 ms', hint: 'baseline' },
      { label: 'Throughput', value: '980 rps', hint: 'sustained' },
      { label: 'Memory', value: '420 MB', hint: 'avg' },
    ]
  };

  go = {
    title: 'Go stack',
    subtitle: 'Go services + goroutines/channels + Kafka client',
    bullets: [
      'Concurrency via goroutines; scheduling & lightweight handlers',
      'Kafka client comparison (sarama / franz-go etc.)',
      'Profiling: pprof'
    ],
    kpis: [
      { label: 'P95', value: '35 ms', hint: 'baseline' },
      { label: 'P99', value: '60 ms', hint: 'baseline' },
      { label: 'Throughput', value: '1120 rps', hint: 'sustained' },
      { label: 'Memory', value: '310 MB', hint: 'avg' },
    ]
  };

  // side-by-side comparison charts (mock values)
  latencyCompare = {
    labels: ['P95 (ms)', 'P99 (ms)'],
    datasets: [
      { data: [42, 75], label: '.NET' },
      { data: [35, 60], label: 'Go' }
    ]
  };

  throughputCompare = {
    labels: ['Throughput (rps)'],
    datasets: [
      { data: [980], label: '.NET' },
      { data: [1120], label: 'Go' }
    ]
  };

  resourceCompare = {
    labels: ['CPU (%)', 'Memory (MB)'],
    datasets: [
      { data: [62, 420], label: '.NET' },
      { data: [48, 310], label: 'Go' }
    ]
  };
}
